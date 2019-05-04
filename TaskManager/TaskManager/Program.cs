using System;
using System.Collections.Generic;

namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> taskList = new List<Task>();
            bool repeat = true;
            taskList = TestData();

            while (repeat)
            {
                try
                {
                    switch (GetInputFromMainMenu())
                    {
                        case 1:
                            Console.Clear();
                            DisplayAllTasks(taskList);
                            Pause();

                            break;

                        case 2:
                            taskList.Add(Task.AddOrEdit(null));
                            UserInput.Display("New task added!");
                            Pause();

                            break;

                        case 3:
                            if (DeleteTask(taskList))
                            {
                                UserInput.Display("\nDeleted!\n\n");
                            }
                            Pause();

                            break;

                        case 4:
                            MarkTaskAsComplete(taskList);
                            Pause();

                            break;

                        case 5:
                            Console.Clear();
                            while (repeat)
                            {
                                int selectedOption = AdvancedOptionMenu();
                                //if user exit advanced option repeat equals false
                                repeat = PerformAdvancedTask(selectedOption, taskList);
                                if(repeat)
                                {
                                    Pause();
                                }
                            }
                            repeat = true;//Set back to true for main menu
                            
                            
                            break;

                        case 6:
                            if (UserInput.UserConfirmationPrompt("Are you sure(Y/N)?"))
                            {
                                repeat = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    UserInput.Display("That record does not exists. " +
                        "Use option 1 to view the list of tasks");
                }
            }//while
            UserInput.Display("Goodbye!\n");
        }

        private static bool PerformAdvancedTask(int selectedOption, List<Task> taskList)
        {
            if (selectedOption == 1)
            {
                DisplayTasksByOwnerName(taskList);
            }
            else if (selectedOption == 2)
            {
                DisplayAllTaskBeforeDate(taskList);
            }
            else if(selectedOption == 3)
            {
                try
                {
                    Task selectedTask = GetSelectedTaskFromUser(taskList,
                        "Enter Task# to edit: ");

                    selectedTask.DisplayTask();

                    if (UserInput.UserConfirmationPrompt("Are you sure(Y/N)"))
                    {
                        selectedTask = Task.AddOrEdit(selectedTask, true);
                        UserInput.Display("Updated!");
                    }
                    else
                    {
                        UserInput.Display("Cancelled by user.");
                    }
                }
                catch(Exception)
                {
                    UserInput.Display("Item does not exists!");
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        private static void Pause()
        {
            UserInput.Display("Press any key to continue.......");
            Console.ReadKey();
        }

        private static void DisplayAllTaskBeforeDate(List<Task> taskList)
        {
            string date = UserInput.GetUserInputAsDate("Find task due by: ");
            List<Task> tasksByDate = FindTasksAfterDate(taskList, date);

            if (tasksByDate.Count > 0)
            {
                foreach (Task task in tasksByDate)
                {
                    task.DisplayTask();
                }
            }
            else
            {
                UserInput.Display("No results found!");
            }
        }

        private static void DisplayTasksByOwnerName(List<Task> taskList)
        {
            List<Task> employeeTasks = FindMemberTasks(taskList,
                                                UserInput.GetUserInput("Enter some or all of employees name: "));

            if (employeeTasks.Count > 0)
            {
                UserInput.Display($"({employeeTasks.Count}) result(s) found.");

                foreach (Task task in employeeTasks)
                {
                    task.DisplayTask();
                }
            }
            else
            {
                UserInput.Display("Nothing found....");
            }
        }

        private static List<Task> FindTasksAfterDate(List<Task> taskList, string date)
        {
            return taskList.FindAll(x => DateTime.Parse(x.dueDate) <= DateTime.Parse(date));
        }

        private static int GetInputFromMainMenu()
        {
            Console.Clear();

            UserInput.Display(" *****ASP DOT NET TASK MANAGER - MAIN MENU***\n");
            #region menu
            UserInput.Display("\t1..... List Tasks");
            UserInput.Display("\t2..... Add Task");
            UserInput.Display("\t3..... Delete Task");
            UserInput.Display("\t4..... Mark Task Complete");
            UserInput.Display("\t5..... Advanced Option");
            UserInput.Display("\t6..... Quit\n\n");
            #endregion
            int input = UserInput.GetUserInputAsInteger("Select a task from list(1-6):  ");

            if(input > 0 && input < 7)
            {
                return input;
            }
            return GetInputFromMainMenu();
        }

        private static int AdvancedOptionMenu()
        {
            Console.Clear();
            //Extra challenges
            UserInput.Display(" ADVANCED OPTION MENU");
            UserInput.Display("\t1..... Search by member");
            UserInput.Display("\t2..... Search by date");
            UserInput.Display("\t3..... Edit Task");
            UserInput.Display("\t4..... Go Back to Main");

            int input = UserInput.GetUserInputAsInteger("Select an advanced option(1-4):  ");

            if(input > 0 && input < 5)
            {
                return input;
            }
            return AdvancedOptionMenu();
        }

        private static List<Task> FindMemberTasks(List<Task> list, string name)
        {
            return list.FindAll(x => x.memberList.Exists(y => 
                                y.name.ToLower().Contains(name.ToLower())));
        }

        private static void DisplayAllTasks(List<Task> taskList)
        {
            int count = 1;

            foreach (Task task in taskList)
            {
                UserInput.Display($"Task#: {count}\n");
                task.DisplayTask();
                count++;
            }
        }

        public static void MarkTaskAsComplete(List<Task> list)
        {
            int[] indexOfTask = UserInput.GetUserMultipleInputAsInteger("Task# to set to complete " +
                "(use space to enter multiple tasks#): ");

            foreach (int index in indexOfTask)
            {
                list[index - 1].DisplayTask();
            }
            if (UserInput.UserConfirmationPrompt("\nAre you sure(Y/N)? "))
            {
                foreach (int index in indexOfTask)
                {
                    list[index - 1].MarkAsComplete();
                }
                UserInput.Display("\nCompleted!\n\n");
            }
            else
            {
                UserInput.Display("\nStatus change cancelled!\n\n");
            }
        }
        public static bool DeleteTask(List<Task> list)
        {
            Task selectedTask = GetSelectedTaskFromUser(list, "Enter task # to delete: ");
            UserInput.Display("Task to be deleted:\n\n");
            selectedTask.DisplayTask();
            if (UserInput.UserConfirmationPrompt("\nAre you sure(Y/N)? "))
            {
                return list.Remove(selectedTask);                
            }
            else
            {
                UserInput.Display("\nDelete cancelled!\n\n");
            }
            return false;
        }

        public static Task GetSelectedTaskFromUser(List<Task> list, string message)
        {
            int indexOfTask = UserInput.GetUserInputAsInteger(message);
            indexOfTask--;

            try
            {
                return list[indexOfTask];
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw e;//Stopped here
            }
        }

        public static List<Task> TestData()
        {
            List<Task> tasks = new List<Task>();

            for (int j = 0; j < 3; j++)
            {
                List<Member> memberList = new List<Member>();
                Task task = new Task();

                for (int i = 0; i < 3; i++)
                {
                    memberList.Add(new Member($"Sean{j}{i+1}"));
                }
                task.memberList = memberList;
                task.description = $"Project{j+1}";
                task.dueDate = string.Format($"0{j+6}/20/2019");
                tasks.Add(task);
            }
            return tasks;
        }
    }
}
