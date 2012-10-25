using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFrames
{
    interface IGenerator // I=interface, generator= self explanitory
    {
        Object generate();
    }

    class StringGenerator : IGenerator
    {
        string m_content;
        public StringGenerator(string content = "") 
        {
            m_content = content;
        }
        public virtual Object generate() { return m_content; }
    }

    class FileGenerator : IGenerator
    {
        string m_FileName;
        public FileGenerator(string FileName)
        {
            m_FileName = FileName;
        }
        public virtual Object generate()
        {
            return System.IO.File.ReadAllText(m_FileName);
        }
    }

    class Generators : List<IGenerator>, IGenerator
    {
        public virtual Object generate()
        {
            var buf = new StringBuilder();
            foreach (var i in this)
            {
                buf.Append(i.generate());
            }
            return buf;
        }
    }

    class Page : IGenerator
    {
        public IGenerator m_Header;
        public IGenerator m_Footer;
        public IGenerator m_Body;
        public IGenerator m_StyleSheet;
        public Object generate()
        {
        return "" +
            "\n<style> " + m_StyleSheet.generate() + "</style>\n\n" +
            m_Body.generate() +
            m_Footer.generate();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\mpuntenn\Documents\Visual Studio 2010\Projects\WebFrames\WebFrames\";
            Page page1 = new Page();
            page1.m_Header = new StringGenerator("\n\n<!DOCTYPE: html>\n");
            var body1 = new Generators();
            body1.Add(new FileGenerator(path + "banner.html"));
            body1.Add(new FileGenerator(path + "menu.html"));
            body1.Add(new FileGenerator(path + "page.html"));
            body1.Add(new StringGenerator("\nMy content 4 Page 001!\n\n"));

            page1.m_Body = body1;
            page1.m_Footer = new StringGenerator("\n<br/>\n</body>\n</html>\n\n");
            page1.m_StyleSheet = new FileGenerator(path + "body.css");

            //
            Page page2 = new Page();
            page2.m_Header = new StringGenerator("\n\n<!DOCTYPE: html>\n");
            var body2 = new Generators();
            body2.Add(new FileGenerator(path + "banner.html"));
            body2.Add(new FileGenerator(path + "menu.html"));
            //   body.Add(new FileGenerator(path + "page.html"));
            body2.Add(new StringGenerator("\nMy content four Page 2:\n\n"));
            body2.Add(new FileGenerator(path + "theText.txt"));

            page2.m_Body = body2;
            page2.m_Footer = new StringGenerator("\n<br/>\n</body>\n</html>\n\n");
            page2.m_StyleSheet = new FileGenerator(path + "body.css");
            //

            Console.WriteLine("\n \n \n page1: " + page1.generate() + "\n\n\npage2: " + page2.generate());
            Console.ReadKey();
        }
    }
}
