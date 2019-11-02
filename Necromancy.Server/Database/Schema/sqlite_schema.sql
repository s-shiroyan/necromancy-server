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
  `id`               INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `account_id`       INTEGER                           NOT NULL,
  `name`             TEXT                              NOT NULL,
  `level`            INTEGER                           NOT NULL,
  `created`          DATETIME                          NOT NULL,  
  `password`         TEXT     DEFAULT NULL,
  CONSTRAINT `fk_nec_soul_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`),
  CONSTRAINT `uq_nec_soul_name` UNIQUE (`name`)
);

CREATE TABLE IF NOT EXISTS `nec_character` (
  `id`               INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
  `account_id`       INTEGER                           NOT NULL,
  `soul_id`          INTEGER                           NOT NULL,
  `character_slot_id` INTEGER                          NOT NULL,
  `name`             TEXT                              NOT NULL,
  `race_id`          BIGINT         UNSIGNED           NOT NULL,
  `sex_id`           BIGINT         UNSIGNED           NOT NULL,
  `hair_id`          SMALLINT                          NOT NULL,
  `hair_color_id`    SMALLINT                          NOT NULL,
  `face_id`          SMALLINT                          NOT NULL,
  `alignment_id`     BIGINT          UNSIGNED          NOT NULL,
  `strength`         INTEGER         UNSIGNED          NOT NULL,
  `vitality`         INTEGER         UNSIGNED          NOT NULL,
  `dexterity`        INTEGER         UNSIGNED          NOT NULL,
  `agility`          INTEGER         UNSIGNED          NOT NULL,
  `intelligence`     INTEGER         UNSIGNED          NOT NULL,
  `piety`            INTEGER         UNSIGNED          NOT NULL,
  `luck`             INTEGER         UNSIGNED          NOT NULL,
  `class_id`         INTEGER         UNSIGNED          NOT NULL,
  `level`            INTEGER                           NOT NULL,
  `created`          DATETIME                          NOT NULL,
  CONSTRAINT `fk_nec_character_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`),
  CONSTRAINT `fk_nec_character_soul_id` FOREIGN KEY (`soul_id`) REFERENCES `nec_soul` (`id`),
  CONSTRAINT `uq_nec_character_name` UNIQUE (`soul_id`, `name`)
);

  CREATE TABLE IF NOT EXISTS `Inventory`(
   `BagID` INTEGER,
   `Gold` INTEGER,
   PRIMARY KEY(`BagID`)
);

CREATE TABLE IF NOT EXISTS `SlotsNumbers`(
   `BagID` INTEGER,
   `BagIndex` INTEGER,
   PRIMARY KEY(`BagID`, `BagIndex`),
   FOREIGN KEY(`BagID`) REFERENCES `Inventory`(`BagID`)
);

CREATE TABLE IF NOT EXISTS `Items`(
   `ItemsID` BIGINT,
   `ItemName` TEXT NOT NULL,
   `ItemType` INT,
   `Physics` SMALLINT,
   `Magic` SMALLINT,
   `EnchantID` INT,
   `Durab` INT,
   `Hardness` TINYINT,
   `MaxDur` INT,
   `Numbers` TINYINT,
   `Level` TINYINT,
   `Splevel` TINYINT,
   `Weight` INT,
   `State` INT,
   PRIMARY KEY(`ItemsID`)
);


CREATE TABLE IF NOT EXISTS `ItemsInSlot`(
   `BagID` INT,
   `BagIndex` INT,
   `ItemsID` INT,
   PRIMARY KEY(`BagID`, `BagIndex`, `ItemsID`),
   FOREIGN KEY(`BagID`, `BagIndex`) REFERENCES `SlotsNumbers`(`BagID`, `BagIndex`),
   FOREIGN KEY(`ItemsID`) REFERENCES Items(`ItemsID`)
);

 CREATE TABLE IF NOT EXISTS QuestRequest(
   QuestID INT,
   SoulLevelMission BYTE,
   QuestName CHAR(50),
   QuestLevel INT,
   TimeLimit INT,
   QuestGiverName CHAR(50),
   RewardEXP INT,
   RewardGold INT,
   NumbersOfItems SMALLINT,
   ItemsType INT,
   PRIMARY KEY(QuestID)
);
