import React, {useCallback} from "react";
import {baseUrl} from "../api-base-url";


export const useNewPostFn = () => useCallback((content, createdAt) => {
        return fetch(`${baseUrl}/api/posts`, {
            headers: {
                Accept: 'application/json, text/plain, */*',
                'Content-Type': 'application/json'
            },
            credentials: "include",
            method: "POST",
            body: JSON.stringify({
                content,
                createdAt: createdAt.toISOString()
            })
        });
    }, []);

