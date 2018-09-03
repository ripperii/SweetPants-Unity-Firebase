using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using System.IO;

public class XML : MonoBehaviour {

	public static string MakeXML(List<List<string>> str, List<string> fields)
        {
            string XML = "";
            for (int i = 0; i < str.Count; i++)
            {
                XML += "<row ";
                for(int j = 0; j < str[i].Count; j++)
                {
                    XML += fields[j] + " = " + str[i][j];
                }
                XML += "></row>";
            }
            return XML;
        }
        public static byte[] XMLtoBytes(string xml)
        {
            byte[] bytes = new byte[xml.Length * sizeof(char)];
            Buffer.BlockCopy(xml.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public static string bytesToXML(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        public static List<XMLentry> readXML(string xml)
        {
            
            List<XMLentry> list = new List<XMLentry>();
            
            
            XmlReader reader = XmlReader.Create(new StringReader(xml));

            while(reader.Read())
            {
               string n = "", v = "";
               List<XMLAttribute> a = new List<XMLAttribute>();
               n = reader.Name;
               v = reader.Value;
               if (reader.HasAttributes)
               {
                  while(reader.MoveToNextAttribute())
                  {
                    a.Add(new XMLAttribute(reader.Name, reader.Value));
                  }
               }
                  
            }

            return list;
        }
    }
public class XMLentry
    {
        public string Name;
        public List<XMLAttribute> Attributes;
        public string Value;

        public XMLentry(string name, List<XMLAttribute> attributes, string value)
        {
            Name = name;
            Attributes = attributes;
            Value = value;
        }
    }
public class XMLAttribute
    {
        public string Name;
        public string Value;

        public XMLAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
