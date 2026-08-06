import TagList from "./TagList"
import TagApi from "../api/TagApi"
import { useState, useEffect } from "react"
import { getPagedTagsByTaskId } from "../api/TagApi"
import TodoListApi from "../api/TodoListApi"

export default functon TaskTags(todoListId, taskId, setTagId) {
    const [token] = useAuth();

    const [tags, setTags] = useState({
        items: [],
        totalItems: 0,
        pageNum: 1,
        pageSize: 10,
        totalPages: 0,
    });

    useEffect(() => {
        getPagedTagsByTaskId(todoListId, taskId, tags.pageNum, tags.pageSize, token)
            .then(data => {
                setTags(data);
            })
    }, [taskId])

    return (
        <section>
            <TagList
                
            />
        </section>
    )
}