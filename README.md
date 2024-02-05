# EasySave

## CODE RULES:

- All variables must be in English; functions and classes should also be in English.
- Add meaningful comments to enhance code reliability and help the next developer understand the code.
- Avoid putting words directly in the code for translation; use XML TRANSLATION.
- When adding a new feature, include corresponding tests.
- Don't work directly in the main branch / create yours 
- Before merging your feature branch, coordinate with a co-worker for code review.

## ISSUE WORKING:

- When starting work on an issue, assign it to yourself and close it when your pull request is validated.

## CONVENTION FOR BRANCH NAMING:

To set your branch name, follow this example:
If working on issue #1, "new button," and your name is CHATARD Gaetan ([Trigramme](https://www.google.com/search?q=trigramme+nom&client=firefox-b-d&sca_esv=5f32bda464ccee7b&sxsrf=ACQVn08sxup9UukyPAabRtHKzU9Euip5wg%3A1707144596359&ei=lPXAZYTHFbagkdUPgtqIyAE&ved=0ahUKEwiE4fuZuZSEAxU2UKQEHQItAhkQ4dUDCA8&uact=5&oq=trigramme+nom&gs_lp=Egxnd3Mtd2l6LXNlcnAiDXRyaWdyYW1tZSBub20yBRAAGIAEMgUQABiABDIGEAAYFhgeMggQABgWGB4YDzIIEAAYFhgeGA9IvRVQ2wRYzRBwAHgFkAEAmAFhoAHmAqoBATS4AQPIAQD4AQL4AQHCAgQQABhHwgIPEAAYgAQYFBiHAhhGGPkBwgIKEAAYgAQYFBiHAsICJhAAGIAEGBQYhwIYRhj5ARiXBRiMBRjdBBhGGPQDGPUDGPYD2AEB4gMEGAAgQYgGAZAGCLoGBggBEAEYEw&sclient=gws-wiz-serp)),
your branch name would be: GCD-#1.

## HOW TO CREATE A BRANCH:

1. Switch to the develop branch:
   ```bash
   
   git switch develop
2. Update your code:
   ```bash
   
   git pull
4. Create your branch:
   ```bash
   
   git checkout -b <YOUR BRANCH NAME>

## PULL REQUEST:

When launching a pull request, demonstrate it with a team member. If validated, close the associated issue and squash your code.
