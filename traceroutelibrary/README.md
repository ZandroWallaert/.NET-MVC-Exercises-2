# Lab Traceroute

This lab will teach how to create and use Libraries.
In **part I** you will transform the traceroute repo into a library.
In **part II** a console application is created that uses the library of part I.


## Part I

In this part you will **not** create a library from scratch. Instead you will turn this given repo into a library.

### How to start:
Which file contains meta information about the project?

### Requirements:
   - **Transform** this repo into a Library.
   - Use the createALibrary.pdf on Leho and the slides of this lesson.
   - Name the library **traceroute_lib**
   - Version number 1.0

### When done:
Upload the created library to gitlab in your student-name folder.

## Part II

Create a **console application** called **networktools**. This console application uses the library of previous part and turns it into a user friendly **command line interface.**

### How to start:
Carefully analyze the original traceroute code. 
Take the traceroute **overview slide** next to you. 
What is the primary class you will use from the traceroute library?

### Requirements:
   - The CLI has two arguments (command and destination).
      - e.g. dotnet run --ping start.howest.be
   - The CLI support the **ping** and **traceroute** command.
   - Make sure the CLI has input validation:
     - Unknown commands are not executed.
     - Missing destination == console warning to the user.
     - Missing command == console warning to the user.

### When done:
Upload the console app **networktools** to gitlab in your student-name folder.

# Hints and Tricks
  - Use the C# cheat sheet on gitlab.
  - Use the dotnet cli cheat sheet on gitlab.
  - Use the git cheat sheet on gitlab.
  - Use the slides and createALibrary.pdf from Leho.


