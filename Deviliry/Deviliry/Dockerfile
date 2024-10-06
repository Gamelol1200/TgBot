# Шаг 1: Используем официальный образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Устанавливаем рабочую директорию внутри контейнера
WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости
COPY ./Deviliry/Deviliry.csproj ./Deviliry/
RUN dotnet restore ./Deviliry/Deviliry.csproj

# Копируем все файлы проекта и компилируем приложение
COPY ./Deviliry/. ./Deviliry/
RUN dotnet publish ./Deviliry/Deviliry.csproj -c Release -o /app

# Шаг 2: Используем более легкий образ .NET Runtime для запуска
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

# Копируем собранное приложение из предыдущего этапа
COPY --from=build /app ./ 

# Указываем команду запуска приложения
ENTRYPOINT ["dotnet", "Deviliry/Deviliry.dll"]  # Измените на имя вашего .dll