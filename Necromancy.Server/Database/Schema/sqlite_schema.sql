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
  `name`             TEXT                              NOT NULL,
  `level`            INTEGER                           NOT NULL,
  `created`          DATETIME                          NOT NULL,  
  CONSTRAINT `fk_nec_character_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`),
  CONSTRAINT `fk_nec_character_soul_id` FOREIGN KEY (`soul_id`) REFERENCES `nec_soul` (`id`),
  CONSTRAINT `uq_nec_character_name` UNIQUE (`name`)
);