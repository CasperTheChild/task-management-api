export default function TagList(tags, setTagId) {
    return (
        <div>
            {tags.items.length > 0 &&
                <div>
                    <h1>Tags:</h1>
                    <ul>
                        {tags.items.map(tag => (
                            <li><button onClick={() => setTagId(tag.id)}>{tag.name}</button></li>
                        ))}
                    </ul>
                </div>
            }
        </div>
    )
}