CREATE TYPE user_roles AS ENUM ('customer', 'manager');
CREATE TYPE task_statuses AS ENUM ('to_do', 'in_progress', 'done');

CREATE TABLE users (
	id serial NOT NULL PRIMARY KEY,
	login varchar(50) NOT NULL,
	password varchar(50) NOT NULL,
	"role" user_roles NOT NULL,
	firstname varchar(100) NOT NULL,
	midname varchar(100) NOT NULL,
	lastname varchar(100) NOT NULL,
	created_at timestamp NOT NULL DEFAULT now(),
	last_change_password_date timestamp NULL
);
CREATE UNIQUE INDEX users_login_idx ON users (login);

CREATE TABLE tasks (
	id bigserial NOT NULL PRIMARY KEY,
	project_number int NOT NULL,
	name varchar(200) NOT NULL,
	description text,
	status task_statuses DEFAULT 'to_do',
	manager_id int NOT NULL,
	customer_id int NULL,
	created_at timestamp NOT NULL DEFAULT now(),
	last_updated_at timestamp NULL
);

CREATE TABLE tasks_logs (
	id bigserial NOT NULL PRIMARY KEY,
	task_id bigserial NOT NULL,
	changed_task_user_id int NOT NULL,
	description text NOT NULL,
	created_at timestamp NOT NULL DEFAULT now(),
	FOREIGN KEY (task_id) REFERENCES tasks (id) ON DELETE CASCADE
);

WITH customersWithActiveTasks as 
(
    SELECT DISTINCT customer_id
    FROM tasks 
    WHERE 
        customer_id is not null 
        and status in ('to_do', 'in_progress')
)
SELECT 
    id,
    login,
    firstname,
    midname,
    lastname
FROM users as u
WHERE u.id nit in (select customer_id from customersWithActiveTasks);
