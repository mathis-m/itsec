import React, {useCallback, useEffect, useState} from "react";
import {Alert} from "@mui/material";

export const useFetchErrorWrapper = (fetchCall, defaultValue) => {
    const [error, setError] = useState(null);
    const [statusCode, setStatusCode] = useState(null);
    const [result, setResult] = useState(defaultValue);
    const [counter, setCounter] = useState(0);
    const fetchAgain = () => {
        setCounter(counter + 1);
    };

    useEffect(() => {
        fetchCall(fetch)
            .then(res => {
                setStatusCode(res.status)
                if (!res.ok) {
                    res.text().then(text => setError(text))
                    return;
                }
                res.json()
                    .then(json => setResult(json))
                    .catch(err => {
                        setError(err)
                    })
            })
            .catch(err => setError(err));
    }, [fetchCall, counter]);

    return [
        result,
        error && (
            <Alert severity="error">{error}</Alert>
        ),
        statusCode,
        fetchAgain
    ]
}
