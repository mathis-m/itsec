import './App.css';
import {
    Box,
    Button,
    Card,
    CardContent,
    Container,
    CssBaseline,
    Stack,
    TextareaAutosize,
    Typography
} from "@mui/material";
import {usePosts} from "./hooks/usePosts";
import {useRef} from "react";

const PostCreate = () => {
    const inputRef = useRef();
    const onShare = (event) => {
        console.log(inputRef.current.value);
    }

    return (
        <Box>
            <TextareaAutosize
                minRows={3}
                placeholder="Your post..."
                style={{ width: "100%" }}
                onClick={onShare}
                ref={inputRef}
            />
            <Button variant="outlined"  onClick={onShare}>Share</Button>
        </Box>
    )
}

const PostList = () => {
    const [posts, error] = usePosts();
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
            <Stack spacing={2} sx={{height: "100%", overflowY: "auto"}}>
                <PostCreate key="creator"/>
                {
                    posts.map(p => (
                        <Card  key={p.id} sx={{overflow: "unset"}}>
                            <CardContent sx={{display: "flex", flexDirection: "column"}}>
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

const App = () => (
    <>
        <CssBaseline/>
        <Container fixed sx={{height: "100%", display: "flex", flexDirection: "column"}}>
            <PostList />
        </Container>
    </>
);

export default App;
