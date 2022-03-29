import React, {useEffect, useState} from "react";
import {Alert} from "@mui/material";

export const useFetchErrorWrapper = (fetchCall, defaultValue) => {
    const [error, setError] = useState(null);
    const [result, setResult] = useState(defaultValue);

    useEffect(() => {
        fetchCall(fetch)
            .then(res => {
                if (!res.ok) {
                    res.text().then(text => setError(text))
                    return;
                }
                res.json()
                    .then(json => setResult(json))
                    .catch(err => setError(err))
            })
            .catch(err => setError(err));
    }, [fetchCall]);

    return [
        result,
        error && (
            <Alert severity="error">{error}</Alert>
        )
    ]
}
