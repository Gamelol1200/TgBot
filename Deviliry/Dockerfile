# Шаг 1: Используем образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Копируем файл решения и проекты для восстановления зависимостей
COPY *.sln ./
COPY DataBase/*.csproj ./DataBase/
COPY Deviliry/*.csproj ./Deviliry/
RUN dotnet restore

# Копируем все файлы проекта и публикуем сборку
COPY . ./
RUN dotnet publish -c Release -o out

# Шаг 2: Используем легкий runtime-образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Копируем результат сборки из предыдущего этапа
COPY --from=build-env /app/out .

# Указываем команду для запуска .dll (проверь, что имя .dll файла совпадает с реальным)
ENTRYPOINT ["dotnet", "Deviliry.dll"]  # Измени название .dll на правильное, если оно другое