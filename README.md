# MnTNotes - .Net Core 5 React with Identity Server 4
## Just Another Notes App
### React Draft Wysiwyg and SQLite

[![GitHub issues](https://img.shields.io/github/issues/MnTNotes/MnTNotes)](https://github.com/MnTNotes/MnTNotes/issues)[![GitHub forks](https://img.shields.io/github/forks/MnTNotes/MnTNotes)](https://github.com/MnTNotes/MnTNotes/network)[![GitHub stars](https://img.shields.io/github/stars/MnTNotes/MnTNotes)](https://github.com/MnTNotes/MnTNotes/stargazers)[![GitHub license](https://img.shields.io/github/license/MnTNotes/MnTNotes)](https://github.com/MnTNotes/MnTNotes/blob/main/LICENSE)

- **Releases**
	- [mntnotes.ml](https://mntnotes.ml/) on Ubuntu 20.04 with Nginx.
    - [mntnotes.azurewebsites.net](https://mntnotes.azurewebsites.net/) on Azure App Service.

- **How to**
    - (Optional) Edit Connection String on "appsettings.json"
    - (Optional) Run "Update-Database" on Package Manager Console

- **Technologies** 
	- .Net Core 5
	- Entity Framework Code-First
	- DataAnnotation & Fluent API
	- React
	- React Draft Wysiwyg
	- Reactstrap
	- Identity Server 4

- **N-Tier architecture**
    - Libraries: Core, Data, Services
    - Presentation: Web, API(will be added), Mobile(will be added)

- **Details**
    - Users can login and register
	- Users can "Show, Add, Edit, Delete Own Notes"

- **DB** \
![DB](/images/0DBDiagram.png)

- **Notes** \
![DB](/images/1Notes.png)

- **Code Maps**
    - Base \
    ![Codemap1](/images/2CodeMap1.png)
    - Presentation Classes \
    ![Codemap2](/images/2CodeMap2.png)
    - Presentation Classes with Properties and Methods \
    ![Codemap3](/images/2CodeMap3.png)
    - Libraries Classes \
    ![Codemap4](/images/2CodeMap4.png)
    - Libraries Classes with Properties and Methods \
    ![Codemap5](/images/2CodeMap5.png)

- **TODO**
	- API and Mobile Application Layer
	- User Profile Page
	- User Roles

- **Acknowledgments**
	- Thanks! [Microsoft](https://github.com/dotnet/core) for .Net Core 5.
	- Thanks! [IdentityServer4](https://identityserver4.readthedocs.io/)
	- Thanks! [reactstrap](https://reactstrap.github.io/)
	- Thanks! [React Draft Wysiwyg](https://github.com/jpuri/react-draft-wysiwyg)
	