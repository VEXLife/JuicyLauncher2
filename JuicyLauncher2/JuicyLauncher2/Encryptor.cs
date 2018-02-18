using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuicyLauncher2
{
    internal class Encryptor
    {
        public String EncryptStr(String input, bool Decrypt = false)
        {
            String[] dict = "C，D，E，2，o，p，q，5，6，7，8，9，0，a，b，c，d，e，f，g，h，i，j，1，r，s，t，u，H，I，V，W，X，Y，l，m，n，G，$，%，J，K，L，M，N，O，P，'，<，,，>，.， ，?，Q，R，S，T，U，^，&，*，(，)，-，_，=，+，{，[，}，]，\\，|，:，;，Z，~，!，@，#，v，w，x，y，z，A，B，F，3，4，k，/".Split("，".ToCharArray());//字典
            String inputStr = input;//准备操作输入字符
            String outputStr = "";//声明输出字符
            if (Decrypt == false)//如果要加密
            {
                for (int a = 0; a < inputStr.Length; a++)//为每个字符循环
                {
                    String dfis = inputStr.Substring(a, 1);//截取字符
                    for (int b = 0; b < dict.Length; b++)//为字典中的每个项循环
                    {
                        if (dict[b] == dfis)//如果找到了项
                        {
                            Random r = new Random();//声明随机数
                            int rv = r.Next(b, dict.Length - 1);//获取一个从索引值到字典所有元素数量的随机数
                            outputStr = outputStr + dict[rv] + dict[rv - b];//存储该数对应的字符和该数减去长度的值对应的字符
                        }
                    }
                }
            }
            else//如果要解密
            {
                for (int a = 0; a < (inputStr.Length / 2); a++)//为每两个字符循环
                {
                    String dfis = inputStr.Substring(2 * a, 2);//截取
                    int rm = 0;//声明被减数
                    int fm = 0;//声明减数
                    for (int b = 0; b < dict.Length; b++)//为字典中的每个项循环
                    {
                        if (dict[b] == dfis.Substring(0, 1))//如果找到了被减数对应的字符
                        {
                            rm = b;//赋值为它对应的值
                        }
                        if (dict[b] == dfis.Substring(1, 1))//如果找到了减数对应的字符
                        {
                            fm = b;//赋值为它对应的值
                        }
                    }
                    outputStr = outputStr + dict[rm - fm];//存储相减的结果
                }
            }
            return outputStr;//返回结果
        }
    }
}
