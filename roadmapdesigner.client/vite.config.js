import { fileURLToPath, URL } from "node:url";
import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// Проверяем, это Docker или нет
const isDocker = process.env.BUILD_IN_DOCKER === "true";

// Настройка HTTPS — по умолчанию выключена
let httpsConfig = undefined;

// Если это не Docker и не production, то пытаемся поднять dev-certs
if (process.env.NODE_ENV !== "production" && !isDocker) {
  import("node:fs").then((fs) => {
    import("node:path").then((path) => {
      import("node:child_process").then(({ spawnSync }) => {
        const baseFolder = process.env.APPDATA
          ? `${process.env.APPDATA}/ASP.NET/https`
          : `${process.env.HOME}/.aspnet/https`;

        const certFile = path.join(baseFolder, `roadmapdesigner.client.pem`);
        const keyFile = path.join(baseFolder, `roadmapdesigner.client.key`);

        if (!fs.existsSync(certFile) || !fs.existsSync(keyFile)) {
          const result = spawnSync(
            "dotnet",
            [
              "dev-certs",
              "https",
              "--export-path",
              certFile,
              "--format",
              "Pem",
              "--no-password",
            ],
            { stdio: "inherit" }
          );

          if (result.status !== 0) {
            throw new Error("Не удалось создать dev-certs сертификаты");
          }
        }

        httpsConfig = {
          key: fs.readFileSync(keyFile),
          cert: fs.readFileSync(certFile),
        };
      });
    });
  });
}

// Настройка прокси для запросов к бэку
const target = process.env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${process.env.ASPNETCORE_HTTPS_PORT}`
  : process.env.ASPNETCORE_URLS?.split(";")[0] ?? "https://localhost:7244";

// Экспорт конфигурации Vite
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  server: {
    proxy: {
      "^/weatherforecast": {
        target,
        secure: false,
      },
    },
    port: 5173,
    https: httpsConfig,
  },
});
