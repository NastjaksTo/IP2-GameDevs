# IP2-GameDevs

**The team**:
Chiara Funke,
Anastasia Kunst,
Andreas Küßner,
Clemens Weber.

## About the project
This is our IP2 Project **Rise of a Legend**. 
For more detailed information about the project have a look at the [projectdokumentation](https://confluence.mni.thm.de/display/SMSIP2SS22G3/SMS+IP-2+SoSe+2022%3A+Gruppe+3+Startseite).
For more detailed information about the code have a look at the [codedokumentation](#).

## Set up the project for development
1. Download and Install Unity(Hub): https://unity3d.com/de/get-unity/download
2. Open UnityHub and login
3. Add your License
4. Install Editor version **2021.3.1f1** - you don't need to add any specific modules, except Visual Studios if you want to use that as your IDE
6. Clone the project (using GitHub)
7. Open the project with Unity
8. You are ready to develop :v: 

## Branches
- development: choose the development branche if you develop. When you are done and you have no errors, you can merge it into the testing branch.
- testing: the testing branch is there to test your code with the rest of the game. GitHub will automatically run tests.
- main: If everything works in the testing branch, you can merge the tesing branch into the main branch. The main branch is only for finished code. GitHub will automatically run tests and build the game. 

## Testing
### 1. via GitHub
Tests are run after each push into the testing branche. 
Under the tab Actions you can see the workflow and results for editmode and playmode.
The tests are defined in the project under IP2-GameDevs/Assets/Tests/EditMode or IP2-GameDevs/Assets/Tests/Playmode.
In IP2-GameDevs/.github/workflows/main.yml you will find the settings to run the tests and the build process automatically in GitHub.

### 2. via Unity
You can run the tests manually inside Unity. 
Open Window/General/Testruner.
Choose Editmode or Playmode (depending on what you want to test) and run it.
The tests are defined in the project under IP2-GameDevs/Assets/Tests/EditMode or IP2-GameDevs/Assets/Tests/Playmode.

## Build and play
### 1. via GitHub
Buildprocess are run after each push into the main branche automatically (it may take long). 
If the build process has been completed successfully, you can click on the Actions tab and select the appropriate workfolw.
Scroll down to the artifacts and download the build folder.
Open the folder and run the exe to start the game.

### 2. via Unity
In the unityeditor you can build the project under File/Build and Run - choose Build and Run or just Build -> Choose a folder.
If you choose Build and Run the game will run automatically after the build has been completed successfully.
If you choose Build you can open the build folder and run the exe to start the game.
