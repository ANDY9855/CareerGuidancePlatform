IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(128) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(128) NOT NULL,
    [FullName] nvarchar(max) NOT NULL,
    [ProfilePicture] nvarchar(max) NULL,
    [Bio] nvarchar(max) NULL,
    [CurrentRole] nvarchar(max) NULL,
    [Industry] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [AssessmentQuestions] (
    [Id] int NOT NULL IDENTITY,
    [QuestionText] nvarchar(max) NOT NULL,
    [Category] nvarchar(max) NOT NULL,
    [OptionA] nvarchar(max) NOT NULL,
    [OptionB] nvarchar(max) NOT NULL,
    [OptionC] nvarchar(max) NOT NULL,
    [OptionD] nvarchar(max) NOT NULL,
    [OrderIndex] int NOT NULL,
    CONSTRAINT [PK_AssessmentQuestions] PRIMARY KEY ([Id])
);

CREATE TABLE [CareerPaths] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Industry] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [RequiredSkills] nvarchar(max) NOT NULL,
    [EducationRequired] nvarchar(max) NOT NULL,
    [AverageSalary] nvarchar(max) NOT NULL,
    [JobOutlook] nvarchar(max) NOT NULL,
    [IconClass] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_CareerPaths] PRIMARY KEY ([Id])
);

CREATE TABLE [JobListings] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Company] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    [Industry] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Requirements] nvarchar(max) NOT NULL,
    [SalaryRange] nvarchar(max) NOT NULL,
    [CompanyLogo] nvarchar(max) NULL,
    [PostedAt] datetime2 NOT NULL,
    [Deadline] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_JobListings] PRIMARY KEY ([Id])
);

CREATE TABLE [JobRoleOverviews] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Industry] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [RequiredSkills] nvarchar(max) NOT NULL,
    [Responsibilities] nvarchar(max) NOT NULL,
    [SalaryRange] nvarchar(max) NOT NULL,
    [JobOutlook] nvarchar(max) NOT NULL,
    [IconClass] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_JobRoleOverviews] PRIMARY KEY ([Id])
);

CREATE TABLE [Mentors] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Specialization] nvarchar(max) NOT NULL,
    [Industry] nvarchar(max) NOT NULL,
    [Bio] nvarchar(max) NOT NULL,
    [ProfilePicture] nvarchar(max) NULL,
    [Email] nvarchar(max) NOT NULL,
    [YearsOfExperience] int NOT NULL,
    [Availability] nvarchar(max) NOT NULL,
    [Rating] float NOT NULL,
    [TotalSessions] int NOT NULL,
    [IsAvailable] bit NOT NULL,
    CONSTRAINT [PK_Mentors] PRIMARY KEY ([Id])
);

CREATE TABLE [NetworkingEvents] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [EventType] nvarchar(max) NOT NULL,
    [Organizer] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [EventDate] datetime2 NOT NULL,
    [RegistrationUrl] nvarchar(max) NULL,
    [IsOnline] bit NOT NULL,
    [Industry] nvarchar(max) NOT NULL,
    [IconClass] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_NetworkingEvents] PRIMARY KEY ([Id])
);

CREATE TABLE [Resources] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    [Industry] nvarchar(max) NOT NULL,
    [SkillCategory] nvarchar(max) NOT NULL,
    [Url] nvarchar(max) NULL,
    [ThumbnailUrl] nvarchar(max) NULL,
    [Provider] nvarchar(max) NOT NULL,
    [IsFree] bit NOT NULL,
    [Duration] nvarchar(max) NULL,
    [DifficultyLevel] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Resources] PRIMARY KEY ([Id])
);

CREATE TABLE [SuccessStories] (
    [Id] int NOT NULL IDENTITY,
    [PersonName] nvarchar(max) NOT NULL,
    [CurrentRole] nvarchar(max) NOT NULL,
    [Company] nvarchar(max) NOT NULL,
    [Story] nvarchar(max) NOT NULL,
    [ProfilePicture] nvarchar(max) NULL,
    [Industry] nvarchar(max) NOT NULL,
    [PublishedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_SuccessStories] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(128) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(128) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(128) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(128) NOT NULL,
    [RoleId] nvarchar(128) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(128) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AssessmentResults] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(128) NOT NULL,
    [TechScore] int NOT NULL,
    [CreativeScore] int NOT NULL,
    [AnalyticalScore] int NOT NULL,
    [LeadershipScore] int NOT NULL,
    [SuggestedCareer] nvarchar(max) NULL,
    [CareerCompatibilityDetails] nvarchar(max) NULL,
    [TakenAt] datetime2 NOT NULL,
    CONSTRAINT [PK_AssessmentResults] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AssessmentResults_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Goals] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(128) NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Category] nvarchar(max) NOT NULL,
    [TermType] nvarchar(max) NOT NULL,
    [ProgressPercent] int NOT NULL,
    [TargetDate] datetime2 NOT NULL,
    [IsCompleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Goals] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Goals_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [PeerPosts] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(128) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [Category] nvarchar(max) NOT NULL,
    [Likes] int NOT NULL,
    [PostedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_PeerPosts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PeerPosts_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ResumeData] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(128) NOT NULL,
    [FullName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Phone] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [LinkedIn] nvarchar(max) NULL,
    [Portfolio] nvarchar(max) NULL,
    [Summary] nvarchar(max) NOT NULL,
    [Skills] nvarchar(max) NOT NULL,
    [Template] nvarchar(max) NOT NULL,
    [LastUpdated] datetime2 NOT NULL,
    CONSTRAINT [PK_ResumeData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ResumeData_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [CareerStages] (
    [Id] int NOT NULL IDENTITY,
    [CareerPathId] int NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Skills] nvarchar(max) NOT NULL,
    [YearsRequired] int NOT NULL,
    [StageOrder] int NOT NULL,
    CONSTRAINT [PK_CareerStages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CareerStages_CareerPaths_CareerPathId] FOREIGN KEY ([CareerPathId]) REFERENCES [CareerPaths] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [EmployerReviews] (
    [Id] int NOT NULL IDENTITY,
    [JobListingId] int NOT NULL,
    [ReviewerName] nvarchar(max) NOT NULL,
    [Rating] int NOT NULL,
    [ReviewText] nvarchar(max) NOT NULL,
    [ReviewedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_EmployerReviews] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EmployerReviews_JobListings_JobListingId] FOREIGN KEY ([JobListingId]) REFERENCES [JobListings] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [JobApplications] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(128) NOT NULL,
    [JobListingId] int NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [AppliedAt] datetime2 NOT NULL,
    [FollowUpDate] datetime2 NULL,
    [Notes] nvarchar(max) NULL,
    CONSTRAINT [PK_JobApplications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_JobApplications_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_JobApplications_JobListings_JobListingId] FOREIGN KEY ([JobListingId]) REFERENCES [JobListings] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [MentorSessions] (
    [Id] int NOT NULL IDENTITY,
    [MentorId] int NOT NULL,
    [UserId] nvarchar(128) NOT NULL,
    [ScheduledAt] datetime2 NOT NULL,
    [SessionType] nvarchar(max) NOT NULL,
    [Topic] nvarchar(max) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [Notes] nvarchar(max) NULL,
    CONSTRAINT [PK_MentorSessions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MentorSessions_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MentorSessions_Mentors_MentorId] FOREIGN KEY ([MentorId]) REFERENCES [Mentors] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [GoalMilestones] (
    [Id] int NOT NULL IDENTITY,
    [GoalId] int NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [IsCompleted] bit NOT NULL,
    [OrderIndex] int NOT NULL,
    CONSTRAINT [PK_GoalMilestones] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GoalMilestones_Goals_GoalId] FOREIGN KEY ([GoalId]) REFERENCES [Goals] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ResumeCertifications] (
    [Id] int NOT NULL IDENTITY,
    [ResumeDataId] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [IssuedBy] nvarchar(max) NOT NULL,
    [Year] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ResumeCertifications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ResumeCertifications_ResumeData_ResumeDataId] FOREIGN KEY ([ResumeDataId]) REFERENCES [ResumeData] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ResumeEducations] (
    [Id] int NOT NULL IDENTITY,
    [ResumeDataId] int NOT NULL,
    [Degree] nvarchar(max) NOT NULL,
    [Institution] nvarchar(max) NOT NULL,
    [Year] nvarchar(max) NOT NULL,
    [GPA] nvarchar(max) NULL,
    CONSTRAINT [PK_ResumeEducations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ResumeEducations_ResumeData_ResumeDataId] FOREIGN KEY ([ResumeDataId]) REFERENCES [ResumeData] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ResumeExperiences] (
    [Id] int NOT NULL IDENTITY,
    [ResumeDataId] int NOT NULL,
    [JobTitle] nvarchar(max) NOT NULL,
    [Company] nvarchar(max) NOT NULL,
    [StartDate] nvarchar(max) NOT NULL,
    [EndDate] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ResumeExperiences] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ResumeExperiences_ResumeData_ResumeDataId] FOREIGN KEY ([ResumeDataId]) REFERENCES [ResumeData] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

CREATE INDEX [IX_AssessmentResults_UserId] ON [AssessmentResults] ([UserId]);

CREATE INDEX [IX_CareerStages_CareerPathId] ON [CareerStages] ([CareerPathId]);

CREATE INDEX [IX_EmployerReviews_JobListingId] ON [EmployerReviews] ([JobListingId]);

CREATE INDEX [IX_GoalMilestones_GoalId] ON [GoalMilestones] ([GoalId]);

CREATE INDEX [IX_Goals_UserId] ON [Goals] ([UserId]);

CREATE INDEX [IX_JobApplications_JobListingId] ON [JobApplications] ([JobListingId]);

CREATE INDEX [IX_JobApplications_UserId] ON [JobApplications] ([UserId]);

CREATE INDEX [IX_MentorSessions_MentorId] ON [MentorSessions] ([MentorId]);

CREATE INDEX [IX_MentorSessions_UserId] ON [MentorSessions] ([UserId]);

CREATE INDEX [IX_PeerPosts_UserId] ON [PeerPosts] ([UserId]);

CREATE INDEX [IX_ResumeCertifications_ResumeDataId] ON [ResumeCertifications] ([ResumeDataId]);

CREATE UNIQUE INDEX [IX_ResumeData_UserId] ON [ResumeData] ([UserId]);

CREATE INDEX [IX_ResumeEducations_ResumeDataId] ON [ResumeEducations] ([ResumeDataId]);

CREATE INDEX [IX_ResumeExperiences_ResumeDataId] ON [ResumeExperiences] ([ResumeDataId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260501123302_InitialCreate', N'9.0.4');

COMMIT;
GO

