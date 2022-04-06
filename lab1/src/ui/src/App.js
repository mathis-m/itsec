import './App.css';
import {
    Alert,
    Avatar,
    Box,
    Button,
    Card,
    CardContent,
    Container,
    CssBaseline, Grid, Link,
    Stack,
    TextareaAutosize, TextField,
    Typography
} from "@mui/material";
import {usePosts} from "./hooks/usePosts";
import React, {useRef, useState} from "react";
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import {useLoginFn} from "./hooks/useLoginFn";
import {useRegisterFn} from "./hooks/useRegisterFn";
import {useNewPostFn} from "./hooks/useNewPostFn";

const PostCreate = ({renewPosts}) => {
    const inputRef = useRef();
    const makePost = useNewPostFn()
    const [error, setError] = useState(null);
    const [statusCode, setStatusCode] = useState(null);
    const onShare = (event) => {
        makePost(inputRef.current.value, new Date)
            .then(res => {
                setStatusCode(res.status)
                if (!res.ok) {
                    res.text().then(text => setError(text && (
                        <Alert severity="error">{text}</Alert>
                    )))
                    return;
                }
                renewPosts()
            })
            .catch(err => setError(err));
    }

    return (
        <Box>
            <TextareaAutosize
                minRows={3}
                placeholder="Your post..."
                style={{ width: "100%" }}
                ref={inputRef}
            />
            <Button variant="outlined"  onClick={onShare}>Share</Button>
        </Box>
    )
}

const PostList = ({posts, error, renewPosts}) => {
    const formatDate = (dateString) => {
        const options = { year: "numeric", month: "long", day: "numeric", hour: "2-digit", minute: "2-digit"}
        return new Date(dateString).toLocaleDateString(undefined, options)
    }
    return (
        <>
            <Typography variant="h1" component="div" gutterBottom>
                ITSEC Lab 1 Hack me
            </Typography>
            <Typography variant="overline" display="block" gutterBottom>
                by robert & mathis
            </Typography>
            {error}
            <Stack spacing={2} sx={{height: "100%", overflowY: "auto", padding: "5px"}}>
                <PostCreate key="creator" renewPosts={renewPosts}/>
                {
                    posts.map(p => (
                        <Card  key={p.id} sx={{overflow: "unset"}}>
                            <CardContent sx={{display: "flex", flexDirection: "column", wordWrap: "break-word"}}>
                                <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                                    {formatDate(p.createdAt)}
                                </Typography>
                                <Typography variant="h5" component="div">
                                    {p.author}
                                </Typography>
                                {/*
                                    A7:2017-Cross-Site Scripting (XSS)
                                    don't render not sanitized string as html :D
                                    fix: <div>{p.content}</div>
                                */}
                                <div style={{flex: "1 1 100%"}} dangerouslySetInnerHTML={{__html: p.content}}/>
                            </CardContent>
                        </Card>
                    ))
                }
            </Stack>
        </>
    )
}

const Login = ({onRegister, onSuccess}) => {
    const loginFn = useLoginFn();
    const [error, setError] = useState(null);
    const [statusCode, setStatusCode] = useState(null);
    const onLogin = (event) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        loginFn(data.get('username'), data.get('password'))
            .then(res => {
                setStatusCode(res.status)
                if (!res.ok) {
                    res.text().then(text => setError(text && (
                        <Alert severity="error">{text}</Alert>
                    )))
                    return;
                }
                res.json()
                    .then(json => onSuccess(json.username))
                    .catch(err => {
                        setError(err)
                    })
            })
    }
    return <Box
        sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
        }}
    >
        {error}
        <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
            <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h5">
            Sign in
        </Typography>
        <Box component="form" onSubmit={onLogin} noValidate sx={{ mt: 1 }}>
            <TextField
                margin="normal"
                required
                fullWidth
                id="username"
                label="Username"
                name="username"
                autoComplete="username"
                autoFocus
            />
            <TextField
                margin="normal"
                required
                fullWidth
                name="password"
                label="Password"
                type="password"
                id="password"
                autoComplete="current-password"
            />
            <Button
                type="submit"
                fullWidth
                variant="contained"
                sx={{ mt: 3, mb: 2 }}
            >
                Sign In
            </Button>
            <Grid container>
                <Grid item>
                    <Typography>
                        Don't have an account?
                    </Typography>
                </Grid>
                <Grid item>
                    <Button onClick={onRegister}>
                         Sign Up
                    </Button>
                </Grid>
            </Grid>
        </Box>
    </Box>
}

const Register = ({onLogin, onSuccess}) => {
    const [error, setError] = useState(null);
    const [statusCode, setStatusCode] = useState(null);
    const registerFn = useRegisterFn();
    const onRegister = (event) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        registerFn(data.get('username'), data.get('password'))
            .then(res => {
                setStatusCode(res.status)
                if (!res.ok) {
                    res.text().then(text => setError(text && (
                        <Alert severity="error">{text}</Alert>
                    )));
                    return;
                }
                res.json()
                    .then(json => onSuccess(json.username))
                    .catch(err => {
                        setError(err)
                    })
            })
    }
    return <Box
        sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
        }}
    >
        <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
            <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h5">
            Sign up
        </Typography>
        <Box component="form" noValidate onSubmit={onRegister} sx={{ mt: 3 }}>
            <Grid container spacing={2}>

                <Grid item xs={12}>
                    <TextField
                        required
                        fullWidth
                        id="username"
                        label="Username"
                        name="username"
                        autoComplete="username"
                    />
                </Grid>
                <Grid item xs={12}>
                    <TextField
                        required
                        fullWidth
                        name="password"
                        label="Password"
                        type="password"
                        id="password"
                        autoComplete="new-password"
                    />
                </Grid>
            </Grid>
            <Button
                type="submit"
                fullWidth
                variant="contained"
                sx={{ mt: 3, mb: 2 }}
            >
                Sign Up
            </Button>
            <Grid container justifyContent="flex-end">
                <Grid item>
                    <Typography>
                        Already have an account?
                    </Typography>
                </Grid>
                <Grid item>
                    <Button onClick={onLogin}>
                         Sign in
                    </Button>
                </Grid>
            </Grid>
        </Box>
    </Box>
}

const App = () => {
    const [posts, postFetchError, statusCode, renewPosts] = usePosts();
    const [isLogin, setIsLogin] = useState(true);
    const [username, setUsername] = useState(null);
    const onNavigateToRegister = () => setIsLogin(false);
    const onNavigateToLogin = () => setIsLogin(true);
    const onLoginSuccess = (username) => {
        setUsername(username);
        renewPosts();
    }
    const doLogout = () => {
        window.cookieStore.delete(".AspNetCore.Cookies")
            .then(() => {
                renewPosts();
                setIsLogin(true);
            });
    }
    return (
        <>
            <CssBaseline/>
            <Container fixed sx={{height: "100%", display: "flex", flexDirection: "column"}}>
                {statusCode === 200 && <Box>
                    <Typography>
                        Welcome {username}
                    </Typography>
                    <Button onClick={doLogout}>Logout</Button></Box>
                }
                {statusCode === 200 && <PostList posts={posts} error={postFetchError} renewPosts={renewPosts}/>}
                {statusCode === 401 && isLogin && <Login onRegister={onNavigateToRegister} onSuccess={onLoginSuccess}/>}
                {statusCode === 401 && !isLogin && <Register onLogin={onNavigateToLogin} onSuccess={onLoginSuccess}/>}
            </Container>
        </>
    );
};

export default App;
