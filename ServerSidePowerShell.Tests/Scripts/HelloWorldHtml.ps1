function Tag{
    param(
        [Parameter(Mandatory = $true, ParameterSetName = "TagOnly", Position = 1)]
        [Parameter(Mandatory = $true, ParameterSetName = "TagAndAttrs", Position = 1)]
        [Parameter(Mandatory = $true, ParameterSetName = "TagAndChildren", Position = 1)]
        [Parameter(Mandatory = $true, ParameterSetName = "Full", Position = 1)]
        [string]$tagName, 
        [Parameter(Mandatory = $true, ParameterSetName = "TagAndAttrs", Position = 2)]
        [Parameter(Mandatory = $true, ParameterSetName = "Full", Position = 2)]
        [hashtable]$attrs, 
        [Parameter(Mandatory = $true, ParameterSetName = "TagAndChildren", Position = 2)]
        [Parameter(Mandatory = $true, ParameterSetName = "Full", Position = 3)]
        [scriptblock]$children)
    "<${tagName}>" 
    $children.Invoke()
    "</$tagName>"
}

function Register-Tag {
    param($tagName)
#    $tagClosure = $Function:global:Tag.Ast.Body
#    Set-Item -Path function:global:$name -Value $tagClosure
    Set-Item -Path function:global:$tagName -Value {
    param(
        [Parameter(Mandatory = $false, ParameterSetName = "TagAndAttrs", Position = 1)]
        [Parameter(Mandatory = $false, ParameterSetName = "Full", Position = 1)]
        [hashtable]$attrs, 
        [Parameter(Mandatory = $false, ParameterSetName = "TagAndChildren", Position = 1)]
        [Parameter(Mandatory = $false, ParameterSetName = "Full", Position = 2)]
        [scriptblock]$children)
        Tag $tagName $attrs $children
    }.GetNewClosure() 
}

Register-Tag html
Register-Tag head
Register-Tag body

Html {
	Head {
	    Script @{language = "javascript"}
	}
	Body {

	}
}
