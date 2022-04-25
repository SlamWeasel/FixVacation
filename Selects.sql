--GetTeam
SELECT 
	TOP 1 [Teams] 
FROM [Users] 
WHERE
	[UserName] = ''



--GetLeaderJobs
SELECT
	*,
	(SELECT TOP 1 [UserName] FROM [Users] WHERE [Users.UserID] = [UserID]) AS [UserName]
FROM [Jobs]
WHERE
	[Recipient] = 
		(SELECT TOP 1 [UserID] WHERE [UserName] = '')
	AND [Stage1Passed] = FALSE
	AND [Aborted] = FALSE
ORDER BY [JobID] ASC



--GetHRJobs
SELECT 
	*,
	(SELECT TOP 1 [UserName] FROM [Users] WHERE [Users.UserID] = [UserID]) AS [UserName]
FROM [Jobs] 
WHERE 
	[Stage1Passed] = TRUE
	AND [Stage2Passed] = FALSE
	AND [Aborted] = FALSE
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
	AND [Stage1Passed] = TRUE
	AND [Aborted] = FALSE
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
	
	CONSTRAINT [JobsPK] PRIMARY KEY ([JobID] ASC),
	CONSTRAINT [JobsFKUsersSen] FOREIGN KEY ([Sender]) REFERENCES [Users]([UserID]),
	CONSTRAINT [JobsFKUsersRec] FOREIGN KEY ([Recipient]) REFERENCES [Users]([UserID])
);
