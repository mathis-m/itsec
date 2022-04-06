import React, {useCallback} from "react";
import {baseUrl} from "../api-base-url";


export const useLoginFn = () => useCallback((username, password) => {
        return fetch(`${baseUrl}/api/users/login`, {
            headers: {
                Accept: 'application/json, text/plain, */*',
                'Content-Type': 'application/json'
            },
            credentials: "include",
            method: "POST",
            body: JSON.stringify({
                username,
                password
            })
        });
    }, []);

