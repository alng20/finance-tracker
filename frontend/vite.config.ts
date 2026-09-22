import babel from "@rolldown/plugin-babel";
import react, { reactCompilerPreset } from "@vitejs/plugin-react";
import { defineConfig } from "vite";

// import basicSsl from "@vitejs/plugin-basic-ssl";

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    babel({ presets: [reactCompilerPreset()] }),
    // basicSsl()
  ],
  server: {
    // Used only for local run, not for docker container
    port: 5173,

    proxy: {
      "/api": {
        target: "http://localhost:5067",
      },
    },
  },
});
