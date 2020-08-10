
 INSERT INTO "nec_union" ("id","name","leader_character_id","subleader1_character_id","subleader2_character_id","level","current_exp","next_level_exp","member_limit_increase","cape_design_id","union_news","created") 
 VALUES 
 (1,'DefaultUnion',1,0,0,0,0,100,0,0,NULL,'2020-02-11 13:26:55.9224609'),
 (2,'Trade_Union',9,2,0,0,0,100,0,0,NULL,'2020-02-11 15:20:31.4537755');

INSERT INTO "nec_union_member" ("id","union_id","character_id","member_priviledge_bitmask","rank","joined") 
VALUES 
 (1,0,1,0,0,'2020-02-11 13:46:17.885088'),
 (4,2,9,103,0,'2020-02-11 15:20:31.5745087'),
 (5,2,2,103,1,'2020-02-11 21:36:07.6324444'),
 (6,2,7,103,2,'2020-02-12 12:29:35.5635632'),
 (20,2,20,103,2,'2020-02-12 12:29:35.5635632'),
 (21,2,21,103,2,'2020-02-12 12:29:35.5635632'),
 (22,2,22,103,2,'2020-02-12 12:29:35.5635632'),
 (23,2,23,103,2,'2020-02-12 12:29:35.5635632'),
 (24,2,5,103,3,'2020-07-30 16:54:58.1731477');


