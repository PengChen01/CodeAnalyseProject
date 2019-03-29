using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Product2.Test
{
    public class Person
    {
        [System.Xml.Serialization.XmlIgnore]
        public Person father;
        public string Name;
        public List<Person> children;
        public Person(string name)
        {
            Name = name;
            children = new List<Person>();
        }
        public Person()
        {
        }
    }
    class High_GradeObjectOriented
    {
        public static void main(RichTextProcess richTextProcess)
        {
            //创建一个叫小明的人,把currentPerson当做指针，指向对象小明
            Person currentPerson = new Person("小明");
            //创建一个家庭，此时家庭就是小明一个人,family指向小明
            Person family = currentPerson;
            //通过孩子小明创建小明的父亲
            currentPerson.father = new Person(currentPerson.Name + "他爸");
            //指定小明的父亲有小明一个孩子
            currentPerson.father.children.Add(currentPerson);
            //在下条语句之前，family内容和person完全一致
            //在下条语句之后，family 的内容为 person.father，没有生成循环结构，family指向对象小明他爸
            //如果使用family.father = currentPerson;则表示小明的父亲就是小明，生成了一种循环结构
            family = currentPerson.father;
            //创建小明的一个孩子
            currentPerson.children.Add(new Person(currentPerson.Name + "的第" + (currentPerson.children.Count + 1) + "个孩子"));
            //孩子的父亲是小明
            currentPerson.children[currentPerson.children.Count - 1].father = currentPerson;
            //创建一个小明的另一个孩子，小芳,此时指针currentPerson指向对象小芳
            currentPerson = new Person("小芳");
            //判断小芳的父亲是否存在，如果不存在，将小芳的父亲指向小明
            if (currentPerson.father == null)
            {
                currentPerson.father = family.children[0];
            }
            //小明的另一个孩子是小芳
            family.children[0].children.Add(currentPerson);
            //此时对象小明有两个孩子1个父亲，小明他爸有一个孩子,小明的两个孩子都有一个共同的父亲小明，他们的家谱关系保存在对象family
            //上一条语句也可以替换成,效果是一样的
            //currentPerson.father.children.Add(currentPerson);

            //当然family不一定指向小明他爸，也可以指向小芳
            family = currentPerson;
            //新建小芳的一个孩子小王
            currentPerson = new Person("小芳的孩子小王");
            //小芳孩子的父亲是小芳
            currentPerson.father = family;
            //小芳有一个孩子
            family.children.Add(currentPerson);

            //计算小王有几代人
            //注意下条语句千万不能写成currentPerson.father = currentPerson ;否则就形成了循环结构，父亲永远是父亲
            int Generation = 1;
            while (currentPerson.father!=null)
            {
                Generation++;
                currentPerson = currentPerson.father;
            }
            //显示对象的XML结构
            richTextProcess.appendObject(currentPerson);
            //由上可知family就相当于一个中间表，防止currentPerson新建对象时，原来的父子关系丢失
        }
    }
}
