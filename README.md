# EasySave

## DEMO

https://github.com/Programmation-Systeme/EasySave/assets/96482486/8ef5e62f-f34d-4d47-aec7-31de5c281448

## Test Convention Guidelines
- Use descriptive names for test methods.
- Follow a consistent naming convention (camel case) for test methods (e.g., `saveFile_WhenFileHasBeenSaved_ReturnsTrue`).
 
## Run Unit TESTS:
1. PRESS Test.

<img src="https://github.com/Programmation-Systeme/EasySave/assets/96482486/a05990ea-6398-4495-b144-bb8d3239c3d0" alt="image" style="width:500px;height:50px;">

3. THEN Test Explorer.
4. Finally, click on Run All Tests in the view.
   
<img src="https://github.com/Programmation-Systeme/EasySave/assets/96482486/5a728c06-c145-4bb5-9b35-ac4a0d328d60" alt="image" style="width:500px;height:300px;">

## CODE RULES:

- All variables must be in English; functions and classes should also be in English.
- Add meaningful comments to enhance code reliability and help the next developer understand the code.
- Avoid putting words directly in the code for translation; use XML TRANSLATION.
- When adding a new feature, include corresponding tests.
- Don't work directly in the main branch / create yours
- Before merging your feature branch, coordinate with a co-worker for code review.

## ISSUE WORKING:

- When starting work on an issue, assign it to yourself and close it when your pull request is validated.

## CONVENTION FOR COMMITS NAMING:

When making commits to your repository, follow this convention for clear and organized commit messages:

- Feature Addition: Use the prefix FEAT: followed by the corresponding issue number and a brief description of the feature added.
  Example: FEAT: #5 "Add new user authentication feature"

- Code Improvement: Utilize the prefix QA: for commits related to code quality assurance or general code improvements. Again, include the issue number and a brief description.
  Example: QA: #5 "Optimize database query for improved performance"

- Bug Fixing: Begin with FIX: followed by the issue number and a concise explanation of the bug fix.
  Example: FIX: #5 "Resolve null pointer exception in user registration"

- Code Refactoring: Preface with REFACTO: followed by the issue number and a clear summary of the refactoring changes.
  Example: REFACTO: #5 "Simplify authentication logic for better maintainability"

## CONVENTION FOR BRANCH NAMING:

To set your branch name, follow this example:
If working on issue #1, "new button," and your name is CHATARD Gaetan,
your branch name would be: Gaetan-#1.

## HOW TO CREATE A BRANCH:

1. Switch to the develop branch:

   ```bash

   git switch develop
   ```

2. Update your code:

   ```bash

   git pull
   ```

3. Create your branch:

   ```bash

   git checkout -b <YOUR BRANCH NAME>
   ```

## PULL REQUEST:

When launching a pull request, demonstrate it with a team member. If validated, close the associated issue and squash your code.

## CONTRIBUTORS

Individuals who have contributed to this project

<table >
  <td align="center">
  <a href="https://github.com/MatteraAurelien">
    <img src="https://avatars.githubusercontent.com/u/72558842?v=4" width="100px;" alt="MATTERAAurelien"/> <br />
    <sub>
      <b>MATTERA Aurelien</b>
    </sub>
  </a>
    <br />
      💅
    </a>
  </td>
  
  <td align="center">
  <a href="https://github.com/EthanTESTA">
    <img src="https://avatars.githubusercontent.com/u/118427837?v=4" width="100px;" alt="TESTAEthan"/> <br />
    <sub>
      <b>TESTA Ethan</b>
    </sub>
  </a>
    <br />
      🧸
    </a>
  </td>
  
  <td align="center">
  <a href="https://github.com/gaetanchatard">
    <img src="https://avatars.githubusercontent.com/u/158268075?s=96&v=4" width="100px;" alt="CHATARDGaetan"/> <br />
    <sub>
      <b>CHATARD Gaetan</b>
    </sub>
  </a>
    <br />
      🐉
    </a>
  </td>
  
  <td align="center">
  <a href="https://github.com/On1zuma">
    <img src="https://avatars.githubusercontent.com/u/96482486?v=4" width="100px;" alt="MESFIOUIWassim"/> <br />
    <sub>
      <b>MESFIOUI Wassim</b>
    </sub>
  </a>
    <br />
      🤯
    </a>
  </td>
  
  <td align="center">
  <a href="https://github.com/EliseeLeydier">
    <img src="https://avatars.githubusercontent.com/u/92147165?v=4" width="100px;" alt="EliseeLeydier"/> <br />
    <sub>
      <b>Leydier Elisée</b>
    </sub>
  </a>
    <br />
      😏
    </a>
  </td>
  
</table>
