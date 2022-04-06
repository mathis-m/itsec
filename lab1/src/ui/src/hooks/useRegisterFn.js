import React, {useCallback} from "react";
import {baseUrl} from "../api-base-url";


export const useRegisterFn = () => useCallback((username, password) => {
        return fetch(`${baseUrl}/api/users/register`, {
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

