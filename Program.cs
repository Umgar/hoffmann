using System;
using System.Collections.Generic;
using System.Linq;

namespace kod_Hoffmana
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            string cont;
            if(args.Length >= 1) input = args[0];
            else {Console.WriteLine("Give an sentence: "); input = Console.ReadLine();}
            List<CharNode> nodes = new List<CharNode>();
            createCode(ref nodes, input);
            foreach(CharNode node in nodes)
                Console.WriteLine($"{node.character} - {node.codeValue}");
            Console.WriteLine(changeToCode(input, nodes));
            do{
                Console.WriteLine("Use this code to encode another strings?");
                Console.WriteLine("1 - continue");
                Console.WriteLine("0 - to end");
                cont = Console.ReadLine();
                if(cont == "0")
                    break;
                Console.WriteLine("Give an sentence: "); 
                input = Console.ReadLine();
                Console.WriteLine(changeToCode(input, nodes));
            }while(true);

        }

        static public void createCode(ref List<CharNode> nodes, string input)
        {
            
            foreach(char letter in input)
            {
                charsToNodes(letter.ToString(), ref nodes);
            }
            IEnumerable<CharNode> nodeIE = nodes.OrderBy(node => node.value).Reverse();
            while(nodeIE.Count() > 1)
            {
                CharNode left = nodeIE.Last();
                nodeIE = nodeIE.Where(node => node != left).Select(node => node);
                CharNode right = nodeIE.Last();
                nodeIE = nodeIE.Where(node => node != right).Select(node => node);
                nodeIE = nodeIE.Append(new CharNode(left.value+right.value, left: left, right: right));
                nodeIE = nodeIE.OrderBy(node => node.value).Reverse();
            }
            nodeIE.Last().Code();
            nodes.Clear();
            nodeIE.Last().Transfer(ref nodes);
        }
        static public string changeToCode(string input, List<CharNode> nodes)
        {
            string output = String.Empty;
            foreach(char charackter in input)
                {
                    foreach(CharNode node in nodes)
                    {
                        if(node.character == charackter.ToString())
                            {output += node.codeValue; break;}
                    }
                }
            return output;
        }
        
        static public void charsToNodes(string ch, ref List<CharNode> nodes)
        {
            if(ch == "")
                return;
            
            foreach(CharNode node in nodes)
                if(node.character == ch)
                    {node.IncreseValue(); return;}
           
            nodes.Add(new CharNode(1, ch));
        }

        public class CharNode{
            public string character{private set; get;}
            public int value{private set; get;}
            public string codeValue{private set; get;}
            public CharNode rightSide{private set; get;}
            public CharNode leftSide{private set; get;}
            public CharNode(int w, string z = "", CharNode left = null, CharNode right = null, string code = "")
            {
                character = z;
                value = w;
                if(right!=null&&left!=null)
                { rightSide=right; leftSide = left;}
                if(code != String.Empty)
                    codeValue = code;
            }
            public void IncreseValue()
            {
                value++;
            }
            public void Code(string codeNow = "")
            {
                if(leftSide.character != "")
                    leftSide.codeValue = codeNow+"0";
                else if(leftSide.character == "")
                    leftSide.Code(codeNow+"0");
                if(rightSide.character != "")
                    rightSide.codeValue = codeNow+"1";
                else if(rightSide.character == "")
                    rightSide.Code(codeNow+"1");
                
                
            }
            public void Transfer(ref List<CharNode> nodes)
            {
                if(leftSide.character != "")
                    nodes.Add(new CharNode(leftSide.value, leftSide.character, code: leftSide.codeValue));
                else if(leftSide.character == "")
                    leftSide.Transfer(ref nodes);
                if(rightSide.character != "")
                    nodes.Add(new CharNode(rightSide.value, rightSide.character, code: rightSide.codeValue));
                else if(rightSide.character == "")
                    rightSide.Transfer(ref nodes);
            }
        }
        
    }
}
