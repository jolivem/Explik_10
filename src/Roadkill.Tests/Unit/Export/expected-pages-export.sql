-- You will need to enable identity inserts for your chosen db before running this Script, for example in SQL Server:
-- SET IDENTITY_INSERT explik_pages ON;

-- Users


-- Pages
INSERT INTO explik_pages (id, title, summary, createdby, createdon, controlledby, publishedon, tags, islocked, issubmitted, iscontrolled, isrejected, iscopied, isvideo, nbrating, totalrating, nbview, filepath, videourl, pseudonym, controllerrating, competitionid) VALUES ('1','Page 1 title','','created-by-user1','2013-01-01','modified-by-user2','2013-01-01','tag1,tag2,tag3','True','False','False','False','False','False','0,''0','0','','','','0','0');
INSERT INTO explik_pages (id, title, summary, createdby, createdon, controlledby, publishedon, tags, islocked, issubmitted, iscontrolled, isrejected, iscopied, isvideo, nbrating, totalrating, nbview, filepath, videourl, pseudonym, controllerrating, competitionid) VALUES ('2','Page 2 title','','created-by-user2','2013-01-02','modified-by-user2','2013-01-02','tagA,tagB,tagC','True','False','False','False','False','False','0,''0','0','','','','0','0');
INSERT INTO explik_pages (id, title, summary, createdby, createdon, controlledby, publishedon, tags, islocked, issubmitted, iscontrolled, isrejected, iscopied, isvideo, nbrating, totalrating, nbview, filepath, videourl, pseudonym, controllerrating, competitionid) VALUES ('3','Page 3 title','','created-by-user3','2013-01-03','modified-by-user3','2013-01-03','tagX,tagY,tagZ','False','False','False','False','False','False','0,''0','0','','','','0','0');

-- Pages contents
INSERT INTO explik_pagecontent (id, pageid, text, controlledby, editedon, versionnumber) VALUES ('13a8ad19-b203-46f5-be10-11e0ebf6f812','1','the text ;''''''


								" more text "','modified-by-user1','2013-01-01 13:00:00','1');
INSERT INTO explik_pagecontent (id, pageid, text, controlledby, editedon, versionnumber) VALUES ('143b0023-329a-49b9-97a4-5094a0e378a2','2','the text ;'''''' #### sdfsdfsdf ####


								" blah text "','modified-by-user2','2013-01-02 13:00:00','1');
INSERT INTO explik_pagecontent (id, pageid, text, controlledby, editedon, versionnumber) VALUES ('15ee19ef-c093-47de-97d2-83dec406d92d','3','the text ;'''''' #### dddd **dddd** ####			
			

								" pppp text "','modified-by-user3','2013-01-03 13:00:00','1');
