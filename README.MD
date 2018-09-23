# ReplicationBackupSolution
Tool to backup MS SQL Server i use for work:  
* Transactional publications
* Jobs
* Table permissions and indexes

# How to use
You need to populate `basedirtobackup.txt` with base directory you want your files to be stored  
And you need to specify sql servers in format `host\instance` with comma between servers  
This tool uses domain authorization, so you need to run .exe from an account with the sysadmin server role  
**It's not usable if you will not change code for you case**
