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
               

        public static Task AddOrEdit(Task task, bool edit = false)
        {
            string name = "name", newDescription, newDueDate;
            
            if (task == null)
            {
                task = new Task();
            }
            List<Member> newMembers = new List<Member>();
            newMembers = task.memberList;

            while (name != string.Empty)
            {
                Console.Write("\n\nAdd a new owner or ENTER to quit: ");

                name = Console.ReadLine();
                if (name != string.Empty)
                {
                    newMembers.Add(new Member(name));
                }
            }
            if (edit)
            {
                newDescription = UserInput.GetUserInput("Press Enter to keep old descrpition: ", false);
                newDueDate = UserInput.GetUserInputAsDate("Press Enter to keep old deadline: ", false);

                if (newDueDate != string.Empty)
                {
                    task.dueDate = newDueDate;
                }

                if (newDescription != string.Empty)
                {
                    task.description = newDescription;
                }
            }
            else
            {
                newDescription = UserInput.GetUserInput("Description: ");
                newDueDate = UserInput.GetUserInputAsDate("Deadline: ");
                return new Task(newDescription, newDueDate, newMembers);
            }
            return task;

        }


        public void MarkAsComplete()
        {
            status = true;
        }

        public void DisplayTask(int index = -1)
        {
            Console.WriteLine($"\n\nTask Members:({memberList.Count}) \n");

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
