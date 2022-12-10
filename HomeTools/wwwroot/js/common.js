function ProcessingServerResponse(data, success, error, common){
    if(data.status === true){
        if(success)
            success();
    } else {
        if(error)
            error();
    }
    if(common)
        common();
}