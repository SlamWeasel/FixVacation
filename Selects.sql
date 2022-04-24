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
