using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DobutsuShogi
{
    class Bencode
    {
        const byte list_byte=(byte)'l';
        const byte end_byte = (byte)'e';
        const byte int_byte = (byte)'i';
        const byte dictionary_byte = (byte)'d';
        static public Encoding enc = Encoding.UTF8;
       
       static internal void write(List <Object> l,Stream s)
        {
            s.WriteByte(list_byte);
            foreach (var obj in l)
            {
                write(obj,s);
            }
            s.WriteByte(end_byte);
        }
       static internal void write(IList l, Stream s)
       {
           s.WriteByte(list_byte);
           foreach (var obj in l)
           {
               write(obj, s);
           }
           s.WriteByte(end_byte);
       }
       static internal void write(Dictionary<string, object> d, Stream s)
        {
            s.WriteByte(dictionary_byte);
            foreach (var obj in d)
            {
                write(obj.Key,s);
                write(obj.Value,s);
            }
            s.WriteByte(end_byte);
        }
       static internal void write(IDictionary d, Stream s)
       {
           s.WriteByte(dictionary_byte);
           
           foreach (var key in d.Keys)
           {

               if (!(key is string) && !(key is byte[])) {
                   throw new ArgumentException("Bencoded dictionary key is not String type: "+key);
               }
               write(key as string, s);
               write(d[key], s);
           }
           s.WriteByte(end_byte);
       }
       static internal void write(Dictionary<byte[], object> d, Stream s)
        {
            s.WriteByte(dictionary_byte);
            foreach (var obj in d)
            {
                write(obj.Key,s);
                write(obj.Value,s);
            }
            s.WriteByte(end_byte);
        }
       static internal void write(object obj, Stream s)
        {
            if (obj.GetType() == typeof(int))
            {
                write((int)obj,s);
            }else if (obj.GetType() == typeof(string))
            {
                write(enc.GetBytes((string)obj),s);
            }
            else if (obj.GetType() == typeof(long))
            {
                write((long)obj,s);
            }
            else if (obj is IList)
            {
                var ilist = obj as IList;
                write(ilist,s);
            }
            else if (obj is IDictionary)
            {
                var idict = obj as IDictionary;
                write(idict, s);
            }
            else if (obj.GetType() == typeof(Dictionary<string,object>))
            {
                write((Dictionary<string, object>)obj,s);
            }
            else if (obj.GetType() == typeof(Dictionary<byte[], object>))
            {
                write((Dictionary<byte[], object>)obj,s);
            }

        }
       static internal void write(int p, Stream s)
        {
            string str = p.ToString();
            s.WriteByte(int_byte);
            byte[] buff = Encoding.ASCII.GetBytes(str);
            s.Write(buff, 0, buff.Length);
            s.WriteByte(end_byte);
        }
       static internal void write(byte[] bytes, Stream s)
        {
            byte[] len = Encoding.ASCII.GetBytes(bytes.Length.ToString());
            s.Write(len, 0, len.Length);
            s.WriteByte((byte)':');
            s.Write(bytes, 0, bytes.Length);
        }
       static internal void write(long l, Stream s)
        {
            string str = l.ToString();
            s.WriteByte(int_byte);
            byte[] buff = Encoding.ASCII.GetBytes(str);
            s.Write(buff, 0, buff.Length);
            s.WriteByte(end_byte);
        }
       static internal object read(Stream input) {
           int b = input.ReadByte();
           
           return read_object(input,b,false);
       }
       static internal object read(Stream input,bool decodeAsString)
       {
           int b = input.ReadByte();

           return read_object(input, b,decodeAsString);
       }
       static private object read_object(Stream input,int b,bool decodeAsString)
       {
           
           if (b < 0) { return null; }
           switch ((byte)b)
           {
               case int_byte:
                   return read_Int(input);
               case list_byte:
                   if (decodeAsString)
                   {
                       return read_Str_List(input);
                   }
                   else
                   {
                       return read_List(input);
                   }
                  // return read_List(input);
               case dictionary_byte:
                   if (decodeAsString) {
                       return read_Str_Dictionary(input);
                   }
                   else
                   {
                       return read_Dictionary(input);
                   }
               default:
                   if(char.IsDigit((char)b)){
                       if (decodeAsString){
                           return enc.GetString(read_byteArray(input, b));
                       }else{
                           return read_byteArray(input, b);
                       }
                   }else{
                   throw new InvalidDataException("Not bencode type '"+(char)b+"'" );
                   }
           }
       }
       internal static Dictionary<byte[], object> readDictionary(Stream input) {
           int b = input.ReadByte();
           if (b != dictionary_byte) { throw new InvalidDataException("Value contains unexpected character '" + (char)b + "'"); }
           return read_Dictionary(input);
       }
       internal static Dictionary<string, object> readStrDictionary(Stream input)
       {
           
           int b = input.ReadByte();
           if (b != dictionary_byte) { throw new InvalidDataException("Value contains unexpected character '" + (char)b + "'"); }
           return read_Str_Dictionary(input);
       }
       private static Dictionary<string, object> read_Str_Dictionary(Stream input)
       {
           Dictionary<string, object> val = new Dictionary<string, object>();
           while (true)
           {
               int b = input.ReadByte();
               if (b == end_byte)
               {
                   break;
               }
               else
               {
                   string key = enc.GetString( read_byteArray(input, b));
                   b = input.ReadByte();
                   val.Add(key, read_object(input, b,true));
               }
           }
           return val;
       }
       private static Dictionary<byte[], object> read_Dictionary(Stream input)
       {
           Dictionary<byte[], object> val = new Dictionary<byte[], object>();
           while (true)
           {
               int b = input.ReadByte();
               if (b == end_byte)
               {
                   break;
               }
               else
               {
                   byte[] key = read_byteArray(input, b);
                   b = input.ReadByte();
                   val.Add(key, read_object(input, b,false));
               }
           }
           return val;
       }

       private static byte[] read_byteArray(Stream input,int first)
       {
              int len = read_len(input,first);
           byte[] rin = new byte[len];
           input.Read(rin, 0, len);
           return rin;
       }
       private static int read_len(Stream input,int first)
       {
           int val = 0;
           char ch = (char)first;
           do
           {
               
               if (char.IsDigit(ch))
               {
                   val = (val * 10) + (int)(ch - '0');
               }
               else if (ch == ':')
               {
                   break;
               }
               else if ((int)ch == -1)
               {
                   throw new InvalidDataException("End of stream");
               }
                ch = (char)input.ReadByte();
           } while (true);
           return val;
       }
       static internal int readInt(Stream input)
       {
           int b = input.ReadByte();
           if (b != int_byte) { throw new InvalidDataException("Value contains unexpected character '"+(char)b+"'"); }
           return read_Int(input);
       }
       static private int read_Int(Stream input) {

           bool negative = false;
           int val = 0;
           while (true)
           {
               char ch=(char)input.ReadByte();
               if (char.IsDigit(ch))
               {
                   
                   val = (val * 10) + (int)(ch - '0');
               }
               else if (ch == end_byte) {
                   break;
               }
               else if (ch == '-')
               {
                   negative = true;
               }
               else if ((int)ch == -1)
               {
                   throw new InvalidDataException("End of stream");
               }

           }
           if (negative) { val = val * -1; }
           return val;
       }
       static internal List<object> readList(Stream input)
       {
           int b = input.ReadByte();
           if (b != list_byte) { throw new InvalidDataException("Value contains unexpected character '" + (char)b + "'"); }
           return read_List(input);
       }
       private static List<object> read_List(Stream input)
       {
           List<object> val = new List<object>();
           while (true)
           {
               int b = input.ReadByte();
               if (b == end_byte)
               {
                   break;
               }
               else {

                   val.Add(read_object(input,b,false));
               }
           }
           return val;
       }
       static internal List<object> readStrList(Stream input)
       {
           int b = input.ReadByte();
           if (b != list_byte) { throw new InvalidDataException("Value contains unexpected character '" + (char)b + "'"); }
           return read_Str_List(input);
       }
       private static List<object> read_Str_List(Stream input)
       {
           List<object> val = new List<object>();
           while (true)
           {
               int b = input.ReadByte();
               if (b == end_byte)
               {
                   break;
               }
               else
               {

                   val.Add(read_object(input, b, true));
               }
           }
           return val;
       }
    
    }
}
