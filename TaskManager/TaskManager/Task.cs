using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager
{
    class Task
    {
        public List<Member> memberList { get; set; }
        public string description { get; set; }        
        public string dueDate { get; set; }
        public bool status { get; set; }

        public Task()
        {
            memberList = new List<Member>();
            status = false;
        }

        public Task(string description, string dueBy, List<Member> members)
        {
            this.status = false;
            this.description = description;
            this.dueDate = dueBy;
            this.memberList = members;
        }
               

        public Task AddOrEdit(bool edit = false)
        {
            string name = "name";

            while (name != string.Empty)
            {
                Console.Write("\n\nAdd a new owner or ENTER to quit: ");

                name = Console.ReadLine();
                if (name != string.Empty)
                {
                    memberList.Add(new Member(name));
                }
            }
            if (edit)
            {
                string newDescription = UserInput.GetUserInput("Press Enter to keep old descrpition: ", false);
                string newDueDate = UserInput.GetUserInputAsDate("Press Enter to keep old deadline: ", false);

                if (newDueDate != string.Empty)
                {
                    dueDate = newDueDate;
                }

                if (newDescription != string.Empty)
                {
                    description = newDescription;
                }
            }
            else
            {
                description = UserInput.GetUserInput("Description: ");
                dueDate = UserInput.GetUserInputAsDate("Deadline: ");
            }
            return this;

        }


        public void MarkAsComplete()
        {
            status = true;
        }

        public void DisplayTask(int index = -1)
        {
            Console.WriteLine($"\n\nTask Owners:({memberList.Count}) \n");

            foreach (Member member in memberList)
            {
                Console.WriteLine($"\t{member.name}");
            }

            Console.WriteLine("\nStatus: {0}", status ? "Complete" : "Incomplete");
            Console.WriteLine($"\nDue By: {dueDate}");
            Console.WriteLine($"\nDescription: {description}");
            UserInput.Display("++++++++++++++++++++++++++++++++++++++++++");
        }
    }
}
