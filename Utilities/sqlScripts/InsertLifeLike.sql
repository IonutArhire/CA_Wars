USE [RuleSets.Development]
GO

INSERT INTO [dbo].[LifeLikes]
           ([Id]
		   ,[Name]
           ,[ForSurvival]
           ,[ForBirth]
           ,[Character])
     VALUES
           (NEWID(),
		    'GOF', 
		    '23',
			'3',
			'Chaotic')
GO

INSERT INTO [dbo].[LifeLikes]
           ([Id]
		   ,[Name]
           ,[ForSurvival]
           ,[ForBirth]
           ,[Character])
     VALUES
           (NEWID(),
		    'Coagulations', 
		    '235678',
			'378',
			'Exploding')
GO


