
# EasySave V3.0 User Guide
## Intro
This is a user guide to the features of the EasySave V3.0 software.
The main feature of the software is to create backup jobs, that is, to duplicate files from one folder to another folder and to allow a simplified update/synchronization of files.
EasySave allows you to configure an infinite number of backup jobs.

## 1. Navigate the software

To navigate through the software, the left side space offers 4 separate menus: "Home", "Create", "Settings" and "Execution".

## 2. Close the software

It is possible to exit the software by clicking on the cross at the top right.

## 3. Select a language

The language can be changed by going to the "Settings" menu and selecting the desired language (French or English). 

## 4. View and interact with backup jobs

The "Home" menu shows the different existing backup jobs and allows to interact with by clicking to select one or more and then clicking on "Run" or "Delete".

## 5. Create a backup job

The "Create" menu allows you to configure a new backup job, just click on "Select source folder" to choose the folder that will be copied and click on "Select destination folder" to choose where this folder will be copied. You must also specify the type of backup that will be applied to this work, complete (all files are even those not modified since the last backup will be copied) and differential (only files that have been modified between the last backup and this one will be copied).
To finalize the creation, just click on "Add".

## 6. Running a backup job

A running backup job can be viewed in the "Run" menu, where it can be canceled by clicking on a job and then on the cross on the right, or stopped and resumed by clicking the pause button and continue.

If several backup jobs are performed at the same time, the operations are performed in parallel (i.e., at the same time).

All files with an extension added in the extensions to be encrypted in the "Settings" menu will be encrypted when running a backup job.

All files with an extension added in the extensions to be prioritized in the "Settings" menu will be copied first when running a backup job.

## 7. Configurable options
You can configure and change several different options in the "Settings" menu:
- Language (choice between French and English)
- The log format (between json and xml)
- Adding file extensions that must be encrypted
- The name of the business process, which if running will prevent any backup work from running
- A maximum size in KB (kilobytes), which will allow if a file of equal or greater size is encountered, to reduce bandwidth by temporarily stopping other running jobs until that file is transferred.
- Adding prioritized file extensions that must be processed first during a backup

## 8. Logs

The logs allow to have a history of the different backups made using EasySave.
The log keeps general information such as:
- The size of the file
- The source file path
- Date last updated
...
The new information is added at the end of the central log file with each update launched.
The log file can be found at the root of the project in the "LogDirectory" folder, separated from the software source code.
