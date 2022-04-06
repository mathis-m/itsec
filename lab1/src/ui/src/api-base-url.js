const prod = {
    url: {
        API_URL: "",
    }
};
const dev = {
    url: {
        API_URL: "http://localhost:5001"
    }
};
export const baseUrl = process.env.NODE_ENV === "development"
    ? dev.url.API_URL
    : prod.url.API_URL;