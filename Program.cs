using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace xmlcreater
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"MyTest.txt";
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(
                    
                    @"\txt_to_xml.txt", FileMode.Open, FileAccess.Read);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                char tab = '\u0009';
                
                int count_line = System.IO.File.ReadAllLines("txt_to_xml.txt").Length;
                
                string[] all_line = System.IO.File.ReadAllLines("txt_to_xml.txt");
                
                int[] tab_count_in_current_string = new int[count_line];
                
                List<string> close_tag = new List<string>();
                
                int k = 0;
                
                for (int i = 0; i < all_line.Length; i++)
                {
                    tab_count_in_current_string[i] = Convert.ToInt32(all_line[i].Count(f => f == tab).ToString());
                    Console.WriteLine("tab_count_in_current_string = " + tab_count_in_current_string[i]);
                }
                
                for (int j = 0; j < all_line.Length - 1; j++)
                {
                    
                    if (tab_count_in_current_string[j] == tab_count_in_current_string[j + 1])
                    {
                        all_line[j] = all_line[j].Replace("\t", "");
                        
                        String repeatedString = new String(tab, tab_count_in_current_string[j]);
                        string elem = @"<" + all_line[j].ToString() + @">" + "\r\n";
                        string elem_close_tag = repeatedString + @"</" + all_line[j].ToString() + @">" + "\r\n";
                        elem = repeatedString + elem + elem_close_tag;
                        File.AppendAllText(path, elem);
                    }
                    
                    else if (tab_count_in_current_string[j] < tab_count_in_current_string[j + 1])
                    {
                        
                        string elem_outer_open_tag = @"<" + all_line[j].ToString() + @">" + "\r\n";
                        elem_outer_open_tag = elem_outer_open_tag.Replace("\t", "");
                        
                        String repeatedString = new String(tab, tab_count_in_current_string[j]);
                        elem_outer_open_tag = repeatedString + elem_outer_open_tag;
                        File.AppendAllText(path, elem_outer_open_tag);

                        elem_outer_open_tag = elem_outer_open_tag.Replace("<", "</");
                        close_tag.Add(elem_outer_open_tag);
                        k = k + 1;
                    }
                    
                    else
                    {
                        string elem_outer_open_tag = @"<" + all_line[j].ToString() + @">" + "\r\n";
                        elem_outer_open_tag = elem_outer_open_tag.Replace("\t", "");
                        
                        String repeatedString = new String(tab, tab_count_in_current_string[j]);
                        elem_outer_open_tag = repeatedString + elem_outer_open_tag;
                        File.AppendAllText(path, elem_outer_open_tag);
                        
                        string elem_close_tag = @"</" + all_line[j].ToString() + @">" + "\r\n";
                        elem_close_tag = elem_close_tag.Replace("\t", "");
                        
                        repeatedString = new String(tab, tab_count_in_current_string[j]);
                        elem_close_tag = repeatedString + elem_close_tag;
                        File.AppendAllText(path, elem_close_tag);
                        
                        string parrents_tag = close_tag[k - 1].ToString() + "\r\n";
                        File.AppendAllText(path, parrents_tag);
                        
                        close_tag.RemoveAt(k - 1);
                        
                        k = k - 1;
                    }
                }

                string repeated_last_String = new String(tab, tab_count_in_current_string[count_line - 1]);
                string last_elem = @"<" + all_line[count_line - 1].ToString() + @">" + "\r\n";
                last_elem = last_elem.Replace("\t", "");
                last_elem = repeated_last_String + last_elem;
                File.AppendAllText(path, last_elem);
                last_elem = last_elem.Replace("<", "</") + "\r\n";
                
                File.AppendAllText(path, last_elem);
                
                for (; k > 0; k--)
                {
                    string parrents_tag = close_tag[k - 1].ToString() + "\r\n";
                    File.AppendAllText(path, parrents_tag);
                }
            }
            Console.ReadKey();
        }
    }
}
