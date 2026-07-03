CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE projects (
    id uuid NOT NULL,
    name character varying(150) NOT NULL,
    description character varying(500),
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone,
    CONSTRAINT "PK_projects" PRIMARY KEY (id)
);

CREATE TABLE activities (
    id uuid NOT NULL,
    project_id uuid NOT NULL,
    name character varying(150) NOT NULL,
    bac numeric(18,2) NOT NULL,
    planned_progress_percent numeric(5,2) NOT NULL,
    actual_progress_percent numeric(5,2) NOT NULL,
    actual_cost numeric(18,2) NOT NULL,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone,
    CONSTRAINT "PK_activities" PRIMARY KEY (id),
    CONSTRAINT ck_activities_actual_cost CHECK (actual_cost >= 0),
    CONSTRAINT ck_activities_actual_percent CHECK (actual_progress_percent >= 0 AND actual_progress_percent <= 100),
    CONSTRAINT ck_activities_bac CHECK (bac >= 0),
    CONSTRAINT ck_activities_planned_percent CHECK (planned_progress_percent >= 0 AND planned_progress_percent <= 100),
    CONSTRAINT "FK_activities_projects_project_id" FOREIGN KEY (project_id) REFERENCES projects (id) ON DELETE CASCADE
);

CREATE INDEX ix_activities_project_id ON activities (project_id);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260703034305_InitialCreate', '10.0.9');

COMMIT;

