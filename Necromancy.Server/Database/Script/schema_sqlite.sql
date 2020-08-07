CREATE TABLE IF NOT EXISTS `setting`
(
    `key`   TEXT NOT NULL,
    `value` TEXT NOT NULL,
    PRIMARY KEY (`key`)
);

CREATE TABLE IF NOT EXISTS `account`
(
    `id`               INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
    `name`             TEXT     NOT NULL,
    `normal_name`      TEXT     NOT NULL,
    `hash`             TEXT     NOT NULL,
    `mail`             TEXT     NOT NULL,
    `mail_verified`    INTEGER  NOT NULL,
    `mail_verified_at` DATETIME DEFAULT NULL,
    `mail_token`       TEXT     DEFAULT NULL,
    `password_token`   TEXT     DEFAULT NULL,
    `state`            INTEGER  NOT NULL,
    `last_login`       DATETIME DEFAULT NULL,
    `created`          DATETIME NOT NULL,
    CONSTRAINT `uq_account_name` UNIQUE (`name`),
    CONSTRAINT `uq_account_normal_name` UNIQUE (`normal_name`),
    CONSTRAINT `uq_account_mail` UNIQUE (`mail`)
);

CREATE TABLE IF NOT EXISTS 'nec_auction_items' (
	'id'	INTEGER NOT NULL,
	'character_id'	INTEGER NOT NULL,
	'item_id'	INTEGER NOT NULL,
	'quantity'	INTEGER NOT NULL,
	'expiry_time'	INTEGER NOT NULL,
	'min_bid'	INTEGER NOT NULL,
	'buyout_price'	INTEGER NOT NULL,
	'bidder_id'	INTEGER,
	'current_bid'	INTEGER,
	'comment'	TEXT,
	PRIMARY KEY('id' AUTOINCREMENT),
	FOREIGN KEY('character_id') REFERENCES 'nec_character'('id') ON DELETE CASCADE,
	FOREIGN KEY('bidder_id') REFERENCES 'nec_character'('id') ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS `nec_soul`
(
    `id`         INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
    `account_id` INTEGER  NOT NULL,
    `name`       TEXT     NOT NULL,
    `level`      INTEGER  NOT NULL,
    `created`    DATETIME NOT NULL,
    `password`   TEXT DEFAULT NULL,
    CONSTRAINT `fk_nec_soul_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`),
    CONSTRAINT `uq_nec_soul_name` UNIQUE (`name`)
);

CREATE TABLE IF NOT EXISTS `nec_map`
(
    `id`          INTEGER NOT NULL PRIMARY KEY,
    `country`     TEXT    NOT NULL,
    `area`        TEXT    NOT NULL,
    `place`       TEXT    NOT NULL,
    `x`           INTEGER NOT NULL,
    `y`           INTEGER DEFAULT NULL,
    `z`           INTEGER DEFAULT NULL,
    `orientation` INTEGER DEFAULT NULL
);

CREATE TABLE IF NOT EXISTS `nec_character`
(
    `id`               INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
    `account_id`       INTEGER  NOT NULL,
    `soul_id`          INTEGER  NOT NULL,
    `slot`             INTEGER  NOT NULL,
    `map_id`           INTEGER  NOT NULL,
    `x`                REAL     NOT NULL,
    `y`                REAL     NOT NULL,
    `z`                REAL     NOT NULL,
    `name`             TEXT     NOT NULL,
    `race_id`          INTEGER  NOT NULL,
    `sex_id`           INTEGER  NOT NULL,
    `hair_id`          INTEGER  NOT NULL,
    `hair_color_id`    INTEGER  NOT NULL,
    `face_id`          INTEGER  NOT NULL,
    `alignment_id`     INTEGER  NOT NULL,
    `strength`         INTEGER  NOT NULL,
    `vitality`         INTEGER  NOT NULL,
    `dexterity`        INTEGER  NOT NULL,
    `agility`          INTEGER  NOT NULL,
    `intelligence`     INTEGER  NOT NULL,
    `piety`            INTEGER  NOT NULL,
    `luck`             INTEGER  NOT NULL,
    `class_id`         INTEGER  NOT NULL,
    `level`            INTEGER  NOT NULL,
    `shortcut_bar0_id` INTEGER  NOT NULL,
    `shortcut_bar1_id` INTEGER  NOT NULL,
    `shortcut_bar2_id` INTEGER  NOT NULL,
    `shortcut_bar3_id` INTEGER  NOT NULL,
    `shortcut_bar4_id` INTEGER  NOT NULL,
    `created`          DATETIME NOT NULL,
    CONSTRAINT `fk_nec_character_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`),
    CONSTRAINT `fk_nec_character_soul_id` FOREIGN KEY (`soul_id`) REFERENCES `nec_soul` (`id`),
    CONSTRAINT `fk_nec_character_map_id` FOREIGN KEY (`map_id`) REFERENCES `nec_map` (`id`),
    CONSTRAINT `uq_nec_character_soul_id_name` UNIQUE (`soul_id`, `name`),
    CONSTRAINT `uq_nec_character_soul_id_slot` UNIQUE (`soul_id`, `slot`)
);

CREATE TABLE IF NOT EXISTS `nec_npc_spawn`
(
    `id`         INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
    `npc_id`     INTEGER  NOT NULL,
    `model_id`   INTEGER  NOT NULL,
    `level`      INTEGER  NOT NULL,
    `name`       TEXT     NOT NULL,
    `title`      TEXT     NOT NULL,
    `map_id`     INTEGER  NOT NULL,
    `x`          REAL     NOT NULL,
    `y`          REAL     NOT NULL,
    `z`          REAL     NOT NULL,
    `active`     INTEGER  NOT NULL,
    `heading`    INTEGER  NOT NULL,
    `size`       INTEGER  NOT NULL,
    `visibility` INTEGER  NOT NULL,
    `created`    DATETIME NOT NULL,
    `updated`    DATETIME NOT NULL,
    `icon`       INTEGER  NOT NULL,
    `status`     INTEGER  NOT NULL,
    `status_x`   INTEGER  NOT NULL,
    `status_y`   INTEGER  NOT NULL,
    `status_z`   INTEGER  NOT NULL
);

CREATE TABLE IF NOT EXISTS `nec_monster_spawn`
(
    `id`         INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
    `monster_id` INTEGER  NOT NULL,
    `model_id`   INTEGER  NOT NULL,
    `level`      INTEGER  NOT NULL,
    `name`       TEXT     NOT NULL,
    `title`      TEXT     NOT NULL,
    `map_id`     INTEGER  NOT NULL,
    `x`          REAL     NOT NULL,
    `y`          REAL     NOT NULL,
    `z`          REAL     NOT NULL,
    `active`     INTEGER  NOT NULL,
    `heading`    INTEGER  NOT NULL,
    `size`       INTEGER  NOT NULL,
    `created`    DATETIME NOT NULL,
    `updated`    DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS `nec_skilltree_item`
(
    `id`       INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    `skill_id` INTEGER                           NOT NULL,
    `char_id`  INTEGER                           NOT NULL,
    `level`    INTEGER                           NOT NULL
);

CREATE TABLE IF NOT EXISTS 'nec_shortcut_bar' (
	'character_id'	INTEGER NOT NULL,
	'bar_num'	INTEGER NOT NULL,
	'slot_num'	INTEGER NOT NULL,
	'shortcut_type'	INTEGER NOT NULL,
	'shortcut_id'	INTEGER NOT NULL,
	PRIMARY KEY('character_id','bar_num','slot_num'),
	FOREIGN KEY('character_id') REFERENCES 'nec_character'('id') ON DELETE CASCADE);

CREATE TABLE IF NOT EXISTS `nec_monster_coords`
(
    `id`         INTEGER PRIMARY KEY NOT NULL,
    `monster_id` INTEGER             NOT NULL,
    `map_id`     INTEGER             NOT NULL,
    `coord_idx`  INTEGER             NOT NULL,
    `x`          REAL                NOT NULL,
    `y`          REAL                NOT NULL,
    `z`          REAL                NOT NULL
);


CREATE TABLE IF NOT EXISTS `nec_block_list`
(
    `id`            INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `soul_id`       INTEGER NOT NULL,
    `block_soul_id` INTEGER NOT NULL,
    FOREIGN KEY (`soul_id`) REFERENCES `nec_soul` (`id`),
    FOREIGN KEY (`block_soul_id`) REFERENCES `nec_soul` (`id`)
);

CREATE TABLE IF NOT EXISTS `nec_union`
(
    `id`                        INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
    `name`                      TEXT     NOT NULL,
    `leader_character_id`       INTEGER  NOT NULL,
    `subleader1_character_id`   INTEGER,
    `subleader2_character_id`   INTEGER,
    `level`                     INTEGER  NOT NULL,
    `current_exp`               INTEGER  NOT NULL,
    `next_level_exp`            INTEGER  NOT NULL,
    `member_limit_increase`     INTEGER  NOT NULL,
    `cape_design_id`            INTEGER,
    `union_news`                TEXT,
    `created`                   DATETIME NOT NULL,
    CONSTRAINT `fk_nec_union_leader_character_id` FOREIGN KEY (`leader_character_id`) REFERENCES `nec_character` (`id`)
    --CONSTRAINT `fk_nec_union_subleader1_character_id` FOREIGN KEY (`subleader1_character_id`) REFERENCES `nec_character` (`id`)
    --CONSTRAINT `fk_nec_union_subleader2_character_id` FOREIGN KEY (`subleader2_character_id`) REFERENCES `nec_character` (`id`)
);

CREATE TABLE IF NOT EXISTS `nec_union_member`
(
    `id`                        INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
    `union_id`                  INTEGER  NOT NULL,
    `character_id`              INTEGER  NOT NULL,
    `member_priviledge_bitmask` INTEGER  NOT NULL,
    `rank`                      INTEGER  NOT NULL,
    `joined`                    DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS `nec_union_news`
(
    `Id`                  INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
    `character_soul_name` STRING  NOT NULL,
    `character_name`      STRING  NOT NULL,
    `activity`            INTEGER NOT NULL,
    `string3`             STRING,
    `string4`             STRING,
    `itemcount`           INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS `nec_black_list`
(
    `id`            INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `soul_id`       INTEGER NOT NULL,
    `black_soul_id` INTEGER NOT NULL,
    FOREIGN KEY (`black_soul_id`) REFERENCES `nec_soul` (`id`),
    FOREIGN KEY (`soul_id`) REFERENCES `nec_soul` (`id`)
);

CREATE TABLE IF NOT EXISTS `nec_friend_list`
(
    `id`             INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `soul_id`        INTEGER NOT NULL,
    `friend_soul_id` INTEGER NOT NULL,
    FOREIGN KEY (`soul_id`) REFERENCES `nec_soul` (`id`),
    FOREIGN KEY (`friend_soul_id`) REFERENCES `nec_soul` (`id`)
);

CREATE TABLE IF NOT EXISTS `nec_gimmick_spawn`
(
    `id`       INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `map_id`   INTEGER NOT NULL,
    `x`        INTEGER NOT NULL,
    `y`        INTEGER NOT NULL,
    `z`        INTEGER NOT NULL,
    `heading`  INTEGER NOT NULL,
    `model_id` INTEGER NOT NULL,
    `state`    INTEGER NOT NULL,
    `created`  DATETIME,
    `updated`  DATETIME
);

CREATE TABLE IF NOT EXISTS `nec_map_transition`
(
    `id`                INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `map_id`            INTEGER NOT NULL,
    `transition_map_id` INTEGER NOT NULL,
    `x`                 REAL    NOT NULL,
    `y`                 REAL    NOT NULL,
    `z`                 REAL    NOT NULL,
    `maplink_heading`   INTEGER NOT NULL,
    `maplink_color`     INTEGER NOT NULL,
    `maplink_offset`    INTEGER NOT NULL,
    `maplink_width`     INTEGER NOT NULL,
    `distance`          INTEGER NOT NULL,
    `left_x`            REAL    NOT NULL,
    `left_y`            REAL    NOT NULL,
    `left_z`            REAL    NOT NULL,
    `right_x`           REAL    NOT NULL,
    `right_y`           REAL    NOT NULL,
    `right_z`           REAL    NOT NULL,
    `invertedPos`       INTEGER NOT NULL,
    `to_x`              REAL    NOT NULL,
    `to_y`              REAL    NOT NULL,
    `to_z`              REAL    NOT NULL,
    `to_heading`        INTEGER NOT NULL,
    `state`             INTEGER NOT NULL,
    `created`           DATETIME,
    `updated`           DATETIME
);

CREATE TABLE IF NOT EXISTS `nec_ggate_spawn`
(
    `id`          INTEGER PRIMARY KEY NOT NULL,
    `serial_id`   INTEGER             NOT NULL,
    `interaction` INTEGER             NOT NULL,
    `name`        TEXT                NOT NULL,
    `title`       TEXT                NOT NULL,
    `map_id`      INTEGER             NOT NULL,
    `x`           REAL                NOT NULL,
    `y`           REAL                NOT NULL,
    `z`           REAL                NOT NULL,
    `heading`     INTEGER             NOT NULL,
    `model_id`    INTEGER             NOT NULL,
    `size`        INTEGER             NOT NULL,
    `active`      INTEGER             NOT NULL,
    `glow`        INTEGER             NOT NULL,
    `created`     DATETIME            NOT NULL,
    `updated`     DATETIME            NOT NULL
);

--Inventory and Item Related tables

CREATE TABLE IF NOT EXISTS `nec_item_library`
(
    `id`                  INTEGER NOT NULL PRIMARY KEY,
    `name`                TEXT    NOT NULL,
    `item_type`           INTEGER NOT NULL,
    `equipment_slot_type` INTEGER NOT NULL,
    `physical`            INTEGER NOT NULL,
    `magical`             INTEGER NOT NULL,
    `durability`          INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS `nec_item_spawn` (
	`id`	                INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	`character_id`	        INTEGER NOT NULL,
	`item_id`	            INTEGER NOT NULL,
	`quantity`	            INTEGER NOT NULL,
	`current_durability`	INTEGER NOT NULL,
    `storage_type`          INTEGER NOT NULL,   --ToDo identify Union location and avatar location bytes
	`bag`	                INTEGER NOT NULL,	
	`slot`	                INTEGER NOT NULL,	
    `state`	                INTEGER NOT NULL,	--equipped, soulbound, at auction, in shop. placeholder for bitmask.
	CONSTRAINT `fk_nec_item_spawn_item_id` FOREIGN KEY(`item_id`) REFERENCES `nec_item_library`(`id`)
);

CREATE TABLE IF NOT EXISTS `nec_item_character_bag` (
	`id`	    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	`char_id`	INTEGER NOT NULL,
	`slot`      INTEGER NOT NULL,
	`size`	    INTEGER NOT NULL,
	CONSTRAINT `fk_nec_item_character_bag_character_id` FOREIGN KEY(`char_id`) REFERENCES `nec_character`(`id`)
);
