#!/bin/bash

# Mengambil argumen
while [[ "$#" -gt 0 ]]; do
    case $1 in
        --name) NAME="$2"; shift ;;
        *) echo "Unknown parameter passed: $1"; exit 1 ;;
    esac
    shift
done

# Memastikan bahwa nama telah diberikan
if [ -z "$NAME" ]; then
    echo "Usage: $0 --name <Name>"
    exit 1
fi

# File untuk interface
INTERFACE_FILE="Data/Repository/Interfaces/I${NAME}Repository.cs"

# Isi file interface
cat <<EOL > $INTERFACE_FILE
using Data.Base;
using Data.Entities;

namespace Data.Repository.Interfaces;


public interface I${NAME}Repository: IBaseRepository<${NAME}>
{
    
}
EOL

echo "File $INTERFACE_FILE telah dibuat."

# File untuk class
CLASS_FILE="Data/Repository/${NAME}Repository.cs"

# Isi file class
cat <<EOL > $CLASS_FILE
using Data.Base;
using Data.DbContexts;
using Data.Entities;
using Data.Repository.Interfaces;

namespace Data.Repository;

public class ${NAME}Repository : BaseRepository<${NAME}>, I${NAME}Repository
{
    public ${NAME}Repository(ConnectionDbContexts context) : base(context) 
    {
    }
}
EOL

echo "File $CLASS_FILE telah dibuat."

# create services


# File untuk interface
DATA_SERVICES_INTERFACE="DataServices/Service/Interfaces/I${NAME}DataService.cs"

# Isi file interface
cat <<EOL > $DATA_SERVICES_INTERFACE
using Data.Entities;
using DataServices.Base;

namespace DataServices.Service.Interfaces;
public interface I${NAME}DataService : IBaseServices<${NAME}>
{
}


EOL

echo "File $DATA_SERVICES_INTERFACE telah dibuat."

# File untuk class
CLASS_FILE_DATA_SERVICE="DataServices/Service/${NAME}DataService.cs"

# Isi file class
cat <<EOL > $CLASS_FILE_DATA_SERVICE
using Data.Base;
using Data.Entities;
using Data.Repository.Interfaces;
using DataServices.Base;
using DataServices.Service.Interfaces;

namespace DataServices.Service;

public class ${NAME}DataService: BaseServices<${NAME}>, I${NAME}DataService
{   
    
    private readonly I${NAME}Repository _repository;

    public ${NAME}DataService(I${NAME}Repository repository) : base(repository)
    {
        _repository = repository;
    }
}
EOL

echo "File $CLASS_FILE_DATA_SERVICE telah dibuat."
