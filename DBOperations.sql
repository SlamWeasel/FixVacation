--GetTeam
SELECT 
	TOP 1 [Teams] 
FROM [Users] 
WHERE
	[UserName] = ''



--GetLeaderJobs
SELECT
	*,
	(SELECT TOP 1 [UserName] FROM [Users] WHERE [Users].[UserID] = [Sender]) AS [SenderName],
	(SELECT TOP 1 [UserName] FROM [Users] WHERE [Users].[UserID] = [Recipient]) AS [RecipientName]
FROM [Jobs]
WHERE
	[Recipient] = 
		(SELECT TOP 1 [UserID] FROM [Users] WHERE [UserName] = '')
	AND [Stage1Passed] = 0
	AND [Aborted] = 0
ORDER BY [JobID] ASC



--GetHRJobs
SELECT 
	*,
	(SELECT TOP 1 [UserName] FROM [Users] WHERE [Users].[UserID] = [UserID]) AS [UserName]
FROM [Jobs] 
WHERE 
	[Stage1Passed] = 1
	AND [Stage2Passed] = 0
	AND [Aborted] = 0
ORDER BY [JobID] ASC



--GetMyTeamVacation
SELECT
	*,
	(SELECT TOP 1 [UserName] FROM [Users] WHERE [Users.UserID] = [UserID]) AS [UserName]
FROM [Jobs]
WHERE
	[Sender] IS IN 
		(SELECT [UserID] FROM [Users] WHERE [TeamID] = 
			(SELECT [TeamID] FROM [Users] WHERE [UserName] = ''
			)
		)
	AND [Stage1Passed] = 1
	AND [Aborted] = 0
	AND ([StartDat] BETWEEN {ts '2022-05-01 00:00:00'} AND {ts '2022-05-30 00:00:00'} OR [EndDat] BETWEEN {ts '2022-05-01 00:00:00'} AND {ts '2022-05-30 00:00:00'})
ORDER BY [JobID] ASC





--CREATES
CREATE TABLE [Teams]
(
	[TeamID]	int		NOT NULL,
	[TeamName]	varchar(50)	NOT NULL,

	CONSTRAINT [TeamsPK] PRIMARY KEY ([TeamID] ASC)
);

CREATE TABLE [Users]
(
	[UserID]	int		NOT NULL,
	[UserName]	varchar(50)	NOT NULL,
	[TeamID]	int		NOT NULL,
	[Birthday]	date		NOT NULL,

	CONSTRAINT [UsersPK] PRIMARY KEY ([UserID] ASC),
	CONSTRAINT [UsersFKTeams] FOREIGN KEY ([TeamID]) REFERENCES [Teams]([TeamID])
);

CREATE TABLE [Jobs]
(
	[JobID]       	bigint 		NOT NULL,
	[Sender]      	int 		NOT NULL,
	[StartDat]    	date 		NOT NULL,
	[EndDat]      	date 		NOT NULL,
	[VacAmaount]  	float 		NOT NULL,
	[Recipient]   	int 		NOT NULL,
	[Stage1Passed]	bit		NOT NULL,
	[Stage2Passed]	bit		NOT NULL,
	[Aborted]     	bit		NOT NULL,
	[Reason]	varchar(50)	NULL,
	
	CONSTRAINT [JobsPK] PRIMARY KEY ([JobID] ASC),
	CONSTRAINT [JobsFKUsersSen] FOREIGN KEY ([Sender]) REFERENCES [Users]([UserID]),
	CONSTRAINT [JobsFKUsersRec] FOREIGN KEY ([Recipient]) REFERENCES [Users]([UserID])
);





--INSERTS
INSERT INTO [Teams]
	SELECT ROW_NUMBER() OVER(ORDER BY [MaGrp] ASC), [MaGrp] FROM [Winsped2009].[dbo].[XXAMaG];

UPDATE Jobs SET Stage1Passed = 1 WHERE JobID = 1
	




--Checks
--Checks if User Exists in Database
IF EXISTS(SELECT * FROM [Users] WHERE [UserName] = '')
BEGIN
	SELECT 'True' AS [Bool]
END
ELSE
BEING
	SELECT 'False' AS [Bool]
END






--Scribbles
SELECT * FROM Jobs
SELECT * FROM Users
SELECT * FROM Teams

SELECT
	(SELECT TOP 1 [UserName] FROM [Users] WHERE [Users].[UserID] = [Sender]) AS [UserName],
	[StartDat],
	[EndDat],
	FORMAT([VacAmount], 'N', 'en-us') AS VacAmount
FROM [Jobs]
WHERE
	[Sender] IN 
		(SELECT [UserID] FROM [Users] WHERE [TeamID] = 
			(SELECT [TeamID] FROM [Users] WHERE [UserName] = 'EVLehmann'
			)
		)
	AND [Stage1Passed] = 1
	AND [Aborted] = 0
	AND ([StartDat] BETWEEN {ts '2022-05-01 00:00:00'} AND {ts '2022-05-30 00:00:00'} OR [EndDat] BETWEEN {ts '2022-05-01 00:00:00'} AND {ts '2022-05-30 00:00:00'})
ORDER BY [JobID] ASC

UPDATE Jobs SET Stage1Passed = 1 WHERE JobID = 1

SELECT MAX(JobID) + 1 FROM Jobs

DELETE FROM Jobs WHERE JobID = 4


SELECT
	*,
	(SELECT TOP 1 [UserName] FROM [Users] WHERE [Users].[UserID] = [Sender]) AS [SenderName],
	(SELECT TOP 1 [UserName] FROM [Users] WHERE [Users].[UserID] = [Recipient]) AS [RecipientName]
FROM [Jobs]
WHERE
	[Recipient] = 
		(SELECT TOP 1 [UserID] FROM [Users] WHERE [UserName] = 'JSchuler')
	AND [Stage1Passed] = 0
	AND [Aborted] = 0
ORDER BY [JobID] ASC
