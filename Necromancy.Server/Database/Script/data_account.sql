INSERT INTO "account" ("id","name","normal_name","hash","mail","mail_verified","mail_verified_at","mail_token","password_token","state","last_login","created") VALUES 
 (1,'Admin','Admin','$2a$10$WT0s8dLPde7CWOSrEGIM2u4Nk.n/SBaAZe0VQ.FK.V/f/D1sIGqRu','Admin',0,NULL,NULL,NULL,1,NULL,'2019-11-19 15:58:21.1738355'),
 (2,'1234','1234','$2a$10$gHAWndIPBBGuTcsyOL2KFufzvPDI/evj18DygBv/QelpLENzALcBi','1234',0,NULL,NULL,NULL,1,NULL,'2019-11-19 16:03:55.2889936'),
 (3,'ff','ff','$2a$10$GhZE/d8hTdh92URBeP5k6eH9HbCWhoDknupYegGpgdcIOfRUoGodq','ff',0,NULL,NULL,NULL,1,NULL,'2019-11-19 16:13:35.4028315');

INSERT INTO "nec_soul" ("id","account_id","name","level","created","password") VALUES 
 (1,1,'Server',0,'2019-11-19 16:00:24.0076961','0000'),
 (2,2,'Xeno.',100,'2019-11-19 16:04:30.5066857','1234'),
 (3,3,'Unknown',100,'2019-11-19 16:14:12.2002506','0000'),
 (4,4,'Unknown1',0,'2019-11-23 10:52:36.6968553','0000');

 INSERT INTO "nec_character" ("id","account_id","soul_id","slot","map_id","x","y","z","name","race_id","sex_id","hair_id","hair_color_id","face_id","alignment_id","strength","vitality","dexterity","agility","intelligence","piety","luck","class_id","level","shortcut_bar0_id","shortcut_bar1_id","shortcut_bar2_id","shortcut_bar3_id","shortcut_bar4_id","created") VALUES 
 (1,1,1,1,1001902,23021.0,-224.0,0.0,'Server',1,1,0,0,0,1,0,0,0,0,0,0,4,3,0,1,2,3,4,5,'2019-11-19 16:01:29.30889'),
 (2,2,2,1,1001902,23021.0,-224.0,0.0,'Talin',3,0,0,5,0,3,0,0,0,0,0,0,6,1,0,6,7,8,9,10,'2019-11-19 16:05:17.8122517'),
 (3,2,2,2,1001902,23021.0,-224.0,0.0,'Zakura',0,1,0,0,4,1,0,0,0,0,0,0,8,0,0,11,12,13,14,15,'2019-11-19 16:11:13.2318326'),
 (4,2,2,0,1001902,23021.0,-224.0,0.0,'Kadred',0,0,0,0,0,3,0,0,0,0,0,0,3,2,0,16,17,18,19,20,'2019-11-19 16:12:46.8206967'),
 (5,3,3,1,1001902,23021.0,-224.0,0.0,'One',4,1,1,1,0,3,0,0,0,0,0,0,9,3,0,21,22,23,24,25,'2019-11-19 16:15:30.9221983');

INSERT INTO "nec_shortcut_bar" ("id","slot0","slot1","slot2","slot3","slot4","slot5","slot6","slot7","slot8","slot9","action0","action1","action2","action3","action4","action5","action6","action7","action8","action9") VALUES 
 (1,12501,12601,-1,-1,11,12506,18,22,12512,2,3,3,-1,-1,4,3,4,4,3,4),
 (2,1,2,4,5,6,7,11,14,15,16,5,5,5,5,5,5,5,5,5,5),
 (3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (6,14101,14302,14803,-1,11,-1,18,22,-1,2,3,3,3,-1,4,-1,4,4,-1,4),
 (7,1,2,4,5,6,7,11,14,15,16,5,5,5,5,5,5,5,5,5,5),
 (8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (11,11101,11201,-1,-1,11,-1,18,22,-1,2,3,3,-1,-1,4,-1,4,4,-1,4),
 (12,1,2,4,5,6,7,11,14,15,16,5,5,5,5,5,5,5,5,5,5),
 (13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (16,13101,13404,-1,-1,11,-1,18,22,-1,2,3,3,-1,-1,4,-1,4,4,-1,4),
 (17,1,2,4,5,6,7,11,14,15,16,5,5,5,5,5,5,5,5,5,5),
 (18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (21,12501,12601,-1,-1,11,-1,18,22,-1,2,3,3,-1,-1,4,-1,4,4,-1,4),
 (22,1,2,4,5,6,7,11,14,15,16,5,5,5,5,5,5,5,5,5,5),
 (23,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (24,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1),
 (25,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1);
