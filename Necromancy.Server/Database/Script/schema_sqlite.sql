CREATE TABLE IF NOT EXISTS `setting` (
  `key`   TEXT NOT NULL,
  `value` TEXT NOT NULL,
  PRIMARY KEY (`key`)
);

CREATE TABLE IF NOT EXISTS `account` (
  `id`               INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `name`             TEXT                              NOT NULL,
  `normal_name`      TEXT                              NOT NULL,
  `hash`             TEXT                              NOT NULL,
  `mail`             TEXT                              NOT NULL,
  `mail_verified`    INTEGER                           NOT NULL,
  `mail_verified_at` DATETIME DEFAULT NULL,
  `mail_token`       TEXT     DEFAULT NULL,
  `password_token`   TEXT     DEFAULT NULL,
  `state`            INTEGER                           NOT NULL,
  `last_login`       DATETIME DEFAULT NULL,
  `created`          DATETIME                          NOT NULL,
  CONSTRAINT `uq_account_name` UNIQUE (`name`),
  CONSTRAINT `uq_account_normal_name` UNIQUE (`normal_name`),
  CONSTRAINT `uq_account_mail` UNIQUE (`mail`)
);

CREATE TABLE IF NOT EXISTS `nec_soul` (
  `id`         INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `account_id` INTEGER                           NOT NULL,
  `name`       TEXT                              NOT NULL,
  `level`      INTEGER                           NOT NULL,
  `created`    DATETIME                          NOT NULL,  
  `password`   TEXT     DEFAULT NULL,
  CONSTRAINT `fk_nec_soul_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`),
  CONSTRAINT `uq_nec_soul_name` UNIQUE (`name`)
);

CREATE TABLE IF NOT EXISTS `nec_character` (
  `id`                INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
  `account_id`        INTEGER                           NOT NULL,
  `soul_id`           INTEGER                           NOT NULL,
  `slot`              INTEGER                           NOT NULL,
  `map_id`            INTEGER                           NOT NULL,
  `x`                 REAL                              NOT NULL,
  `y`                 REAL                              NOT NULL,
  `z`                 REAL                              NOT NULL,
  `name`              TEXT                              NOT NULL,
  `race_id`           INTEGER                           NOT NULL,
  `sex_id`            INTEGER                           NOT NULL,
  `hair_id`           INTEGER                           NOT NULL,
  `hair_color_id`     INTEGER                           NOT NULL,
  `face_id`           INTEGER                           NOT NULL,
  `alignment_id`      INTEGER                           NOT NULL,
  `strength`          INTEGER                           NOT NULL,
  `vitality`          INTEGER                           NOT NULL,
  `dexterity`         INTEGER                           NOT NULL,
  `agility`           INTEGER                           NOT NULL,
  `intelligence`      INTEGER                           NOT NULL,
  `piety`             INTEGER                           NOT NULL,
  `luck`              INTEGER                           NOT NULL,
  `class_id`          INTEGER                           NOT NULL,
  `level`             INTEGER                           NOT NULL,
  `created`           DATETIME                          NOT NULL,
  CONSTRAINT `fk_nec_character_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`),
  CONSTRAINT `fk_nec_character_soul_id` FOREIGN KEY (`soul_id`) REFERENCES `nec_soul` (`id`),
  CONSTRAINT `uq_nec_character_soul_id_name` UNIQUE (`soul_id`, `name`),
  CONSTRAINT `uq_nec_character_soul_id_slot` UNIQUE (`soul_id`, `slot`)
);

CREATE TABLE IF NOT EXISTS "nec_npc_spawn" (
	"id"	                        INTEGER             NOT NULL UNIQUE,
	"npc_id"	                    INTEGER             NOT NULL,
	"name"	                        TEXT,
	"status_effect_id"	            INTEGER             NOT NULL DEFAULT (0),
	"status_effect_x"	            REAL                NOT NULL DEFAULT (0),
	"status_effect_y"	            REAL                NOT NULL DEFAULT (0),
	"status_effect_z"	            REAL                NOT NULL DEFAULT (0),
	"to_do"	                        TEXT                         DEFAULT AddedByNPCCommand,
	"special_attribute"	            INTEGER                      DEFAULT (0),
	"event_first_encounter"	        INTEGER                      DEFAULT (0),
	"event_always"	                INTEGER                      DEFAULT (5),
	"play_cutscene_first_encounter"	INTEGER                      DEFAULT (0),
	"play_cutscene_always"	        INTEGER                      DEFAULT (0),
	"level"	                        INTEGER             NOT NULL DEFAULT (99),
	"title"	                        TEXT,
	"dragon_statue_type"	        INTEGER                      DEFAULT (0),
	"icon_type"	                    INTEGER             NOT NULL DEFAULT (231),
	"x"	                            REAL                NOT NULL DEFAULT (100),
	"y"	                            REAL                NOT NULL DEFAULT (100),
	"z"	                            REAL                NOT NULL DEFAULT (1000),
	"map_id"	                    INTEGER                NOT NULL DEFAULT (9999999),
	"display_condition_flag"	    INTEGER             NOT NULL DEFAULT (1),
	"split_map_number"	            INTEGER             NOT NULL DEFAULT (-1),
	"setting_type_flag"	            INTEGER             NOT NULL DEFAULT (6),
	"model_id"	                    INTEGER             NOT NULL DEFAULT (1002000),
	"radius"	                    INTEGER             NOT NULL DEFAULT (45),
	"height"	                    INTEGER             NOT NULL DEFAULT (200),
	"crouch_height"	                INTEGER             NOT NULL DEFAULT (120),
	"name_plate"	                INTEGER             NOT NULL DEFAULT (180),
	"height_model_attribute"	    INTEGER             NOT NULL DEFAULT (1),
	"z_offset"	                    INTEGER             NOT NULL DEFAULT (45),
	"effect_scaling"	            INTEGER             NOT NULL DEFAULT (1),
	"heading"	                    INTEGER             NOT NULL DEFAULT (0),
	"created"	                    DATETIME,
	"updated"	                    DATETIME,
	PRIMARY KEY("Id")
);


CREATE TABLE IF NOT EXISTS `nec_monster_spawn` (
  `id`         INTEGER PRIMARY KEY NOT NULL, 
  `monster_id` INTEGER             NOT NULL,
  `model_id`   INTEGER             NOT NULL,
  `level`      INTEGER             NOT NULL,
  `name`       TEXT                NOT NULL,
  `title`      TEXT                NOT NULL,
  `map_id`     INTEGER             NOT NULL,
  `x`          REAL                NOT NULL,
  `y`          REAL                NOT NULL,
  `z`          REAL                NOT NULL,
  `active`     INTEGER             NOT NULL,
  `heading`    INTEGER             NOT NULL,
  `size`       INTEGER             NOT NULL,
  `created`    DATETIME            NOT NULL,
  `updated`    DATETIME            NOT NULL
);

CREATE TABLE IF NOT EXISTS `nec_item` (
  `id`   INTEGER PRIMARY KEY NOT NULL, 
  `name` TEXT                NOT NULL
);
