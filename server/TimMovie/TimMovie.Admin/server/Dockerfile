FROM node:18.0.0-alpine as BUILD

WORKDIR /app

COPY package.json .

RUN npm install

COPY . .

RUN npm run build

FROM node:18.0.0-alpine as PROD

ENV NODE_ENV=production

WORKDIR /app

COPY package*.json /app/

RUN npm install --only=production

COPY .env.production .

COPY ormconfig.json .

COPY --from=BUILD /app/dist/ .

ENTRYPOINT ["node", "src/main.js"]