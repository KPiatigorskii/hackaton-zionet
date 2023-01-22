1) create instance of twitterEngine
2) go to DB by API and see table twitter-search
{id author_id participant_id event_name team_name engine_port engine_cron_uuid is_searching already_found}

foreach(record in dbRecords){
    if (!record.already_found) // if we need to search
        {
            if (record.is_searching ){ // if search already run
                var is_active = http.request(url, record.port)
                if (!is_active) // if instance is dead
                    newInstance = runNewInstance();
                    rewriteRecord(record.id, newInstance.port)
            }
        }

}