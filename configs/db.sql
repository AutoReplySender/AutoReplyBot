CREATE SCHEMA auto_reply;

CREATE TABLE auto_reply.processed_comments (
    band_no integer NOT NULL,
    post_no integer NOT NULL,
    comment_id integer NOT NULL,
    sub_comment_id integer NOT NULL
);

--
-- Name: processed_comments pk_processed_comments; Type: CONSTRAINT; Schema: auto_reply; Owner: postgres
--

ALTER TABLE ONLY auto_reply.processed_comments
    ADD CONSTRAINT pk_processed_comments PRIMARY KEY (band_no, post_no, comment_id, sub_comment_id);
